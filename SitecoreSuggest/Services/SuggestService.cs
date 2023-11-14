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
        /// Gets or sets the endpoint.
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Gets or sets the API key.
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Gets or sets the max tokens.
        /// </summary>
        public int MaxTokens { get; set; }

        /// <summary>
        /// Gets or sets the reserved tokens.
        /// </summary>
        public int ReservedTokens { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the service supports context.
        /// </summary>
        public bool SupportsContext => Endpoint.Equals(Constants.Chat);

        /// <summary>
        /// Initializes a new instance of the <see cref="SuggestService"/> class.
        /// </summary>
        public SuggestService(string baseUrl = null, string endpoint = null, string model = null , string apiKey = null, int? maxTokens = null, int? reservedTokens = null)
        {
            BaseUrl = baseUrl ?? Sitecore.Configuration.Settings.GetSetting(Constants.BaseUrlSetting);
            Assert.IsNotNull(BaseUrl, nameof(BaseUrl));

            Endpoint = endpoint ?? Sitecore.Configuration.Settings.GetSetting(Constants.EndpointSetting);
            if (!IsEndpointSupported(Endpoint))
                throw new ArgumentException($"Unsupported endpoint: {Endpoint}");

            Model = model ?? Sitecore.Configuration.Settings.GetSetting(Constants.ModelSetting);
            Assert.IsNotNull(Model, nameof(baseUrl));

            ApiKey = apiKey ?? Sitecore.Configuration.Settings.GetSetting(Constants.ApiKeySetting);
            Assert.IsNotNull(ApiKey, nameof(apiKey));

            MaxTokens = maxTokens ?? Sitecore.Configuration.Settings.GetIntSetting(Constants.MaxTokensSetting, 4096);
            ReservedTokens = maxTokens ?? Sitecore.Configuration.Settings.GetIntSetting(Constants.ReservedTokensSetting, 1024);

            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ApiKey}");
        }

        /// <summary>
        /// Generates a suggestion based on a payload.
        /// </summary>
        public string GenerateSuggestion(string prompt, string[] context, float temperature, Language language)
        {
            if (string.IsNullOrEmpty(prompt))
                return string.Empty;

            if (Endpoint.Equals(Constants.Completions))
                return GenerateCompletions(prompt, temperature, Model, language);

            if (Endpoint.Equals(Constants.Chat))
                return GenerateChat(prompt, context, temperature, Model, language);

            throw new ArgumentException($"Unsupported endpoint: {Endpoint}");
        }

        /// <summary>
        /// Generates the completions response.
        /// </summary>
        private string GenerateCompletions(string prompt, float temperature, string model, Language language)
        {
            var requestBody = new CompletionsRequest()
            {
                Prompt = prompt,
                Temperature = temperature,
                Model = model,
                MaxTokens = MaxTokens - prompt.EstimateTokens(language) 
            };

            var jsonResponse = GetResponse(requestBody, string.Concat(BaseUrl, "/completions"), out var errorMessage);
            if (!string.IsNullOrEmpty(errorMessage))
                return errorMessage;
            return jsonResponse?.choices[0]?.text?.ToString()?.Trim() ?? string.Empty;
        }

        /// <summary>
        /// Generates the chat response.
        /// </summary>
        private string GenerateChat(string prompt, string[] context, float temperature, string model, Language language)
        {
            var requestBody = new ChatRequest()
            {
                Temperature = temperature,
                Model = model
            };

            if (context != null)
                requestBody.Messages.AddRange(context.Select(c => new ChatMessage("system", c)));

            // So if the MaxTokens is 4097 and our prompt is 10 tokens and we want to reserve 1024 tokens for the response
            // We will add 3063 tokens worth of context

            if (context != null)
            { 
                var contextTokens = MaxTokens - (ReservedTokens + prompt.EstimateTokens(language));
                foreach (var c in context)
                {
                    contextTokens =- c.EstimateTokens(language);

                    if (contextTokens < 0)
                        break;

                    requestBody.Messages.Add(new ChatMessage("system", c));
                }
            }

            requestBody.Messages.Add(new ChatMessage("user", prompt));

            requestBody.MaxTokens = MaxTokens - requestBody.Messages.Sum(m => m.Content.EstimateTokens(language));

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