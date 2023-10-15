using System.Collections.Generic;

namespace SitecoreSuggest
{
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
        /// The model Sitecore setting
        /// </summary>
        public static string ModelSetting = "SitecoreSuggest.Model";

        /// <summary>
        /// The default number of words in a suggestion
        /// </summary>
        public static int DefaultWords = 2000;

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
    }
}