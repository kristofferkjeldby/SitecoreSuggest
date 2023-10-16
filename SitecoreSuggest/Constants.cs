namespace SitecoreSuggest
{
    using System.Collections.Generic;

    /// <summary>
    /// Constants
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The supported fields (fields that we would like to use suggestions in)
        /// </summary>
        public static string[] SupportedTypeKeys = new string[] {
            "multi-line text",
            "single-line text",
            "rich text"
        };

        /// <summary>
        /// The supported summary field (fields that we would like use for generating summaries)
        /// </summary>
        public static string[] SummaryFields = new string[] {
            "single-line text",
        };

        /// <summary>
        /// The prompts we use to generate language specific summaries
        /// </summary>
        public static Dictionary<string, string> SummaryPrompts = new Dictionary<string, string>
        {
            { "en", "Write summary of \"{0}\"" },
            { "da", "Skriv en opsummering om \"{0}\""}
        };

        /// <summary>
        /// The fields we need to support HTML for
        /// </summary>
        public static string[] HtmlTypeKeys = new string[] {
            "rich text"
        };

        /// <summary>
        /// The API key Sitecore setting
        /// </summary>
        public static string ApiKeySetting = "SitecoreSuggest.ApiKey";

        /// <summary>
        /// The endpoint Sitecore setting
        /// </summary>
        public static string EndpointSetting = "SitecoreSuggest.Endpoint";

        /// <summary>
        /// The default model Sitecore setting
        /// </summary>
        public static string DefaultModelSetting = "SitecoreSuggest.DefaultModel";

        /// <summary>
        /// The temperature Sitecore setting
        /// </summary>
        public static string TemperatureSetting = "SitecoreSuggest.Temperature";

        /// <summary>
        /// The default number of words in a suggestion
        /// </summary>
        public static int DefaultWords = 2000;

        /// <summary>
        /// The default temperature
        /// </summary>
        public static float DefaultTemperature = 0.5f;

        /// <summary>
        /// The words per token (this is for ChatGPT around 0.75 words per token in English)
        /// </summary>
        public static float WordsPerToken = 0.75f;

        /// <summary>
        /// When generating summaries we will display this number of chars in the dropdown
        /// </summary>
        public static int ShortValueLength = 70;

        /// <summary>
        /// The replace keyword
        /// </summary>
        public static string Replace = "replace";

        /// <summary>
        /// The append keyword
        /// </summary>
        public static string Append = "append";

        /// <summary>
        /// The supported chat models
        /// </summary>
        public static string[] SupportedChatModels = new string[] { 
            "gpt-4",
            "gpt-4-0613",
            "gpt-4-32k",
            "gpt-4-32k-0613",
            "gpt-3.5-turbo",
            "gpt-3.5-turbo-0613",
            "gpt-3.5-turbo-16k",
            "gpt-3.5-turbo-16k-0613"
        };

        /// <summary>
        /// The supported completions models
        /// </summary>
        public static string[] SupportedCompletionModels = new string[] { 
            "davinci-002", 
            "babbage-002", 
            "text-davinci-003", 
            "text-davinci-002", 
            "text-davinci-001", 
            "text-curie-001", 
            "text-babbage-001", 
            "text-ada-001", 
            "davinci", 
            "curie", 
            "babbage", 
            "ada"
        };
    }
}