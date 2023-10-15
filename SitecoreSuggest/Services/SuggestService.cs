namespace SitecoreSuggest.Service
{
    using Newtonsoft.Json;
    using Sitecore.Diagnostics;
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
        /// Initializes a new instance of the <see cref="SuggestService"/> class.
        /// </summary>
        public SuggestService(string endpoint = null, string model = null, string apiKey = null)
        {
            Endpoint = endpoint ?? Sitecore.Configuration.Settings.GetSetting(Constants.EndpointSetting);
            Assert.IsNotNull(Endpoint, nameof(endpoint));

            Model = model ?? Sitecore.Configuration.Settings.GetSetting(Constants.ModelSetting);
            Assert.IsNotNull(Model, nameof(model));

            ApiKey = apiKey ?? Sitecore.Configuration.Settings.GetSetting(Constants.ApiKeySetting);
            Assert.IsNotNull(ApiKey, nameof(apiKey));

            httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ApiKey}");
        }

        /// <summary>
        /// Generates a suggestion based on a payload.
        /// </summary>
        public string GenerateSuggestion(string prompt, int words)
        {
            if (string.IsNullOrEmpty(prompt))
                return string.Empty;

            var requestBody = new
            {
                prompt,
                max_tokens = (int)(words / Constants.WordsPerToken),
                n = 1,
                stop = (string)null,
                temperature = 0.7,
                model = Model
            };

            var stringBody = JsonConvert.SerializeObject(requestBody);

            var content = new StringContent(stringBody, Encoding.UTF8, "application/json");

            var response = httpClient.PostAsync(Endpoint, content).Result;

            var jsonResponse = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result ?? string.Empty);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return $"{response.StatusCode}: {jsonResponse?.error?.message}";
            }

            return jsonResponse?.choices[0]?.text?.ToString()?.Trim() ?? string.Empty;
        }
    }
}