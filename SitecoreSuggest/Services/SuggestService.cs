namespace SitecoreSuggest.Service
{
    using Newtonsoft.Json;
    using Sitecore.Diagnostics;
    using Sitecore.Extensions;
    using SitecoreSuggest.Extensions;
    using SitecoreSuggest.Models;
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;

    /// <summary>
    /// Implementation of a suggest service
    /// </summary>
    public class SuggestService
    {
        private readonly HttpClient httpClient;

        /// <summary>
        /// Gets or sets the endpoint.
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the API key.
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Gets or sets the completions model.
        /// </summary>
        public string CompletionsModel { get; set; }

        /// <summary>
        /// Gets or sets the completions model max tokens.
        /// </summary>
        public int CompletionsMaxTokens { get; set; }

        /// <summary>
        /// Gets or sets the chat model.
        /// </summary>
        public string ChatModel { get; set; }

        /// <summary>
        /// Gets or sets the chat model max tokens.
        /// </summary>
        public int ChatMaxTokens { get; set; }

        /// <summary>
        /// Gets or sets the reserved tokens.
        /// </summary>
        public int ReservedTokens { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SuggestService"/> class.
        /// </summary>
        public SuggestService(string baseUrl = null, string apiKey = null, string completionsModel = null, int? completionsMaxTokens = null, string chatModel = null, int? chatMaxTokens = null, int? reservedTokens = null)
        {
            BaseUrl = baseUrl ?? Sitecore.Configuration.Settings.GetSetting(Constants.BaseUrlSetting);
            Assert.IsNotNull(BaseUrl, nameof(BaseUrl));

            CompletionsModel = completionsModel ?? Sitecore.Configuration.Settings.GetSetting(Constants.CompletionsModelSetting);
            Assert.IsNotNull(CompletionsModel, nameof(CompletionsModel));

            ChatModel = chatModel ?? Sitecore.Configuration.Settings.GetSetting(Constants.ChatModelSetting);
            Assert.IsNotNull(ChatModel, nameof(ChatModel));

            ApiKey = apiKey ?? Sitecore.Configuration.Settings.GetSetting(Constants.ApiKeySetting);
            Assert.IsNotNull(ApiKey, nameof(ApiKey));

            CompletionsMaxTokens = completionsMaxTokens ?? Sitecore.Configuration.Settings.GetIntSetting(Constants.CompletionsMaxTokensSetting, 4096);
            ChatMaxTokens = chatMaxTokens ?? Sitecore.Configuration.Settings.GetIntSetting(Constants.ChatMaxTokensSetting, 4096);

            ReservedTokens = reservedTokens ?? Sitecore.Configuration.Settings.GetIntSetting(Constants.ReservedTokensSetting, 1024);

            httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(Constants.TimeOut);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ApiKey}");
        }

        /// <summary>
        /// Generates a suggestion based on a payload.
        /// </summary>
        public string GenerateSuggestion(string prompt, string[] context, float temperature)
        {
            if (string.IsNullOrEmpty(prompt))
                return string.Empty;

            if (context == null || !context.Any())
                return GenerateCompletions(prompt, temperature, CompletionsModel);
            else
                return GenerateChat(prompt, context, temperature, ChatModel);
        }

        /// <summary>
        /// Generates the completions response.
        /// </summary>
        private string GenerateCompletions(string prompt, float temperature, string model)
        {
            var requestBody = new CompletionsRequest()
            {
                Prompt = prompt,
                Temperature = temperature,
                Model = model,
                MaxTokens = CompletionsMaxTokens - prompt.CountTokens() 
            };

            var jsonResponse = GetResponse(requestBody, string.Concat(BaseUrl, "/completions"), out var errorMessage);
            if (!string.IsNullOrEmpty(errorMessage))
                return errorMessage;
            return jsonResponse?.choices[0]?.text?.ToString()?.Trim() ?? string.Empty;
        }

        /// <summary>
        /// Generates the chat response.
        /// </summary>
        private string GenerateChat(string prompt, string[] context, float temperature, string model)
        {
            var requestBody = new ChatRequest()
            {
                Temperature = temperature,
                Model = model
            };

            if (context != null)
                requestBody.Messages.AddRange(context.Select(c => new ChatMessage("system", c)));

            if (context != null)
            { 
                var contextTokens = ChatMaxTokens - (ReservedTokens + prompt.CountTokens());
                foreach (var c in context)
                {
                    contextTokens =- c.CountTokens();

                    if (contextTokens < 0)
                        break;

                    requestBody.Messages.Add(new ChatMessage("system", c));
                }
            }

            requestBody.Messages.Add(new ChatMessage("user", prompt));

            var jsonResponse = GetResponse(requestBody, string.Concat(BaseUrl, "/chat/completions"), out var errorMessage);
            if (!string.IsNullOrEmpty(errorMessage))
                return errorMessage;
            return jsonResponse?.choices[0]?.message?.content?.ToString()?.Trim() ?? string.Empty;
        }

        /// <summary>
        /// Gets the response.
        /// </summary>
        private dynamic GetResponse(dynamic requestBody, string endpoint, out string errorMessage)
        {
            errorMessage = null;
            var stringBody = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(stringBody, Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync(endpoint, content).Result;
            var jsonResponse = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result ?? string.Empty);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                errorMessage = $"{response.StatusCode}: {jsonResponse?.error?.message}";
                return null;
            }

            return jsonResponse;
        }

        /// <summary>
        /// Determines whether a endpoint is supported.
        /// </summary>
        private bool IsEndpointSupported(string endpoint)
        {
            return endpoint.Equals(Constants.Completions) || endpoint.Equals(Constants.Chat);
        }
    }
}