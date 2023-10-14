namespace SitecoreSuggest.Service
{
    using Newtonsoft.Json;
    using Sitecore.Diagnostics;
    using SitecoreSuggest.Extensions;
    using SitecoreSuggest.Models;
    using System.Net;
    using System.Net.Http;
    using System.Text;

    public class SuggestService
    {
        public string GenerateSuggestion(SuggestFormPayload payload)
        {
            var apiKey = Sitecore.Configuration.Settings.GetSetting(Constants.ApiKeySetting);
            Assert.IsNotNull(apiKey, nameof(apiKey));

            var endpoint = Sitecore.Configuration.Settings.GetSetting(Constants.EndpointSetting);
            Assert.IsNotNull(endpoint, nameof(apiKey));

            var prompt = payload.GetPrompt();

            if (string.IsNullOrEmpty(prompt))
                return string.Empty;


            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var requestBody = new
            {
                prompt,
                max_tokens = (int)(payload.Words * (1 / Constants.WordsPerToken)),
                n = 1,
                stop = (string)null,
                temperature = 0.7,
            };

            var stringBody = JsonConvert.SerializeObject(requestBody);

            var content = new StringContent(stringBody, Encoding.UTF8, "application/json");

            var response = client.PostAsync(endpoint, content).Result;

            var jsonResponse = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result ?? string.Empty);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return $"{response.StatusCode} {jsonResponse?.ReasonPhrase}";
            }

            return jsonResponse?.choices[0]?.text?.ToString()?.Trim() ?? string.Empty;
        }
    }
}