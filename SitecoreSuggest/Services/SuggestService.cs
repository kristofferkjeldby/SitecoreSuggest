namespace SitecoreSuggest.Service
{
    using Newtonsoft.Json;
    using Sitecore.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System;
    using Sitecore.Text;

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
        /// Gets or sets the model type.
        /// </summary>
        public string ModelType { get; set; }

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
        /// Initializes a new instance of the <see cref="SuggestService"/> class.
        /// </summary>
        public SuggestService(string endpoint = null, string modelType = null, string model = null, string apiKey = null, int? maxTokens = null)
        {
            Endpoint = endpoint ?? Sitecore.Configuration.Settings.GetSetting(Constants.EndpointSetting);
            Assert.IsNotNull(Endpoint, nameof(endpoint));

            ModelType = modelType ?? Sitecore.Configuration.Settings.GetSetting(Constants.ModelTypeSetting);
            if (!IsModelTypeSupported(ModelType))
                throw new ArgumentException($"Unsupported model type: {Model}");

            Model = model ?? Sitecore.Configuration.Settings.GetSetting(Constants.ModelSetting);
            Assert.IsNotNull(Model, nameof(endpoint));

            ApiKey = apiKey ?? Sitecore.Configuration.Settings.GetSetting(Constants.ApiKeySetting);
            Assert.IsNotNull(ApiKey, nameof(apiKey));

            MaxTokens = maxTokens ?? Sitecore.Configuration.Settings.GetIntSetting(Constants.MaxTokensSetting, 4096);

            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ApiKey}");
        }

        /// <summary>
        /// Generates a suggestion based on a payload.
        /// </summary>
        public string GenerateSuggestion(string prompt, float temperature, string modelType = null, string model = null)
        {
            if (string.IsNullOrEmpty(prompt))
                return string.Empty;

            if (string.IsNullOrWhiteSpace(modelType))
                modelType = ModelType;

            if (string.IsNullOrWhiteSpace(model))
                model = Model;

            if (IsChatModel(modelType))
                return GenerateChat(prompt, temperature, model);

            if (IsCompletionModel(modelType))
                return GenerateCompletions(prompt, temperature, model);

            throw new ArgumentException($"Unsupported model type: {ModelType}");
        }

        /// <summary>
        /// Generates the completions response.
        /// </summary>
        private string GenerateCompletions(string prompt, float temperature, string model)
        {
            var requestBody = new
            {
                prompt,
                max_tokens = MaxTokens - prompt.Length,
                n = 1,
                stop = (string)null,
                temperature,
                model
            };

            var jsonResponse = GetResponse(requestBody, string.Concat(Endpoint, "/completions"), out var errorMessage);
            if (!string.IsNullOrEmpty(errorMessage))
                return errorMessage;
            return jsonResponse?.choices[0]?.text?.ToString()?.Trim() ?? string.Empty;
        }

        /// <summary>
        /// Generates the chat response.
        /// </summary>
        private string GenerateChat(string prompt, float temperature, string model)
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
                max_tokens = MaxTokens - prompt.Length,
                n = 1,
                stop = (string)null,
                temperature,
                model
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
        /// Determines whether a model type is supported.
        /// </summary>
        private bool IsModelTypeSupported(string modelType)
        {
            return IsChatModel(modelType) || IsCompletionModel(modelType);
        }

        /// <summary>
        /// Determines whether a model is a completion model.
        /// </summary>
        private bool IsChatModel(string modelType)
        {
            return modelType.Equals(Constants.Chat);
        }

        /// <summary>
        /// Determines whether a model is a chat model.
        /// </summary>
        private bool IsCompletionModel(string modelType)
        {
            return modelType.Equals(Constants.Completion);
        }
    }
}