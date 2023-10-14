namespace SitecoreSuggest.Models
{
    using Newtonsoft.Json;

    public class SuggestFormPayload
    {
        [JsonProperty("itemId")]
        public string ItemId { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("database")]
        public string Database { get; set; }

        [JsonProperty("fieldId")]
        public string FieldId { get; set; }

        [JsonProperty("promptFieldId")]
        public string PromptFieldId { get; set; }

        [JsonProperty("prompt")]
        public string Prompt { get; set; }

        [JsonProperty("words")]
        public int Words { get; set; }

        [JsonProperty("suggestion")]
        public string Suggestion { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }
    }
}