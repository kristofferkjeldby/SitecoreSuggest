namespace SitecoreSuggest.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// Model for a suggestion form payload. This is the object that the transfered between the javascript and Sitecore
    /// </summary>
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

        [JsonProperty("suggestion")]
        public string Suggestion { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }
    }
}