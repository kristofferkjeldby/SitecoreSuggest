namespace SitecoreSuggest.Service
{
    using Newtonsoft.Json;
    using Sitecore.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System;

    /// <summary>
    /// Implementation of a suggest service
    /// </summary>
    public class SuggestService
    {
        private readonly HttpClient httpClient;

        /// <summary>
        /// Gets or sets the endpoint.
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// Gets or sets the default model.
        /// </summary>
        public string DefaultModel { get; set; }

        /// <summary>
        /// Gets or sets the API key.
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SuggestService"/> class.
        /// </summary>
        public SuggestService(string endpoint = null, string defaultModel = null, string apiKey = null)
        {
            Endpoint = endpoint ?? Sitecore.Configuration.Settings.GetSetting(Constants.EndpointSetting);
            Assert.IsNotNull(Endpoint, nameof(endpoint));

            DefaultModel = defaultModel ?? Sitecore.Configuration.Settings.GetSetting(Constants.ModelSetting);
            Assert.IsNotNull(Endpoint, nameof(endpoint));

            if (!IsModelSupported(DefaultModel))
                throw new ArgumentException($"Unsupported default model: {DefaultModel}");

            ApiKey = apiKey ?? Sitecore.Configuration.Settings.GetSetting(Constants.ApiKeySetting);
            Assert.IsNotNull(ApiKey, nameof(apiKey));

            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ApiKey}");
        }

        /// <summary>
        /// Generates a suggestion based on a payload.
        /// </summary>
        public string GenerateSuggestion(string prompt, int words, float temperature, string model = null)
        {
            if (string.IsNullOrEmpty(prompt))
                return string.Empty;

            if (string.IsNullOrWhiteSpace(model))
                model = DefaultModel;

            if (IsChatModel(model))
                return GenerateChat(prompt, words, temperature);

            if (IsCompletionModel(model))
                return GenerateCompletions(prompt, words, temperature);

            throw new ArgumentException($"Unsupported model: {DefaultModel}");
        }

        /// <summary>
        /// Generates the completions response.
        /// </summary>
        private string GenerateCompletions(string prompt, int words, float temperature)
        {
            var requestBody = new
            {
                prompt,
                max_tokens = (int)(words / Constants.WordsPerToken),
                n = 1,
                stop = (string)null,
                temperature,
                model = DefaultModel
            };

            var jsonResponse = GetResponse(requestBody, string.Concat(Endpoint, "/completions"), out var errorMessage);
            if (!string.IsNullOrEmpty(errorMessage))
                return errorMessage;
            return jsonResponse?.choices[0]?.text?.ToString()?.Trim() ?? string.Empty;
        }

        /// <summary>
        /// Generates the chat response.
        /// </summary>
        private string GenerateChat(string prompt, int words, float temperature)
        {
            var requestBody = new
            {
                messages = new[]
                {
                    new
                    {
                        role = "user",
                        content = prompt
                    }
                },
                max_tokens = (int)(words / Constants.WordsPerToken),
                n = 1,
                stop = (string)null,
                temperature,
                model = DefaultModel
            };

            var jsonResponse = GetResponse(requestBody, string.Concat(Endpoint, "/chat/completions"), out var errorMessage);
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
        /// Determines whether a model is supported.
        /// </summary>
        private bool IsModelSupported(string model)
        {
            if (IsChatModel(model))
                return true;
            if (IsCompletionModel(model))
                return true;
            return false;
        }

        /// <summary>
        /// Determines whether a model is a completion model.
        /// </summary>
        private bool IsChatModel(string model)
        {
            return Constants.SupportedChatModels.Contains(model);
        }

        /// <summary>
        /// Determines whether a model is a chat model.
        /// </summary>
        private bool IsCompletionModel(string model)
        {
            return Constants.SupportedCompletionModels.Contains(model);
        }
    }
}