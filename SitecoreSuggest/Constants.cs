namespace SitecoreSuggest
{
    /// <summary>
    /// Constants
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The supported fields for inserting (fields that we would like to use suggestions in)
        /// </summary>
        public static string[] InsertFields = new string[] {
            "multi-line text",
            "single-line text",
            "rich text"
        };

        /// <summary>
        /// The supported fields for summaries  (fields that we would like use for generating summaries)
        /// </summary>
        public static string[] SummaryFields = new string[] {
            "single-line text",
        };

        /// <summary>
        /// The supported context fields (fields will be sent as part of the context)
        /// </summary>
        public static string[] ContextFields = new string[] {
            "multi-line text",
            "single-line text",
            "rich text"
        };

        /// <summary>
        /// The fields we need to support HTML for
        /// </summary>
        public static string[] HtmlFields = new string[] {
            "rich text"
        };

        /// <summary>
        /// The API key Sitecore setting
        /// </summary>
        public static string ApiKeySetting = "SitecoreSuggest.ApiKey";

        /// <summary>
        /// The base url Sitecore setting
        /// </summary>
        public static string BaseUrlSetting = "SitecoreSuggest.BaseUrl";

        /// <summary>
        /// The endpoint Sitecore setting
        /// </summary>
        public static string EndpointSetting = "SitecoreSuggest.Endpoint";

        /// <summary>
        /// The model Sitecore setting
        /// </summary>
        public static string ModelSetting = "SitecoreSuggest.Model";

        /// <summary>
        /// The max tokens Sitecore setting
        /// </summary>
        public static string MaxTokensSetting = "SitecoreSuggest.MaxTokens";

        /// <summary>
        /// The reserved token Sitecore setting (for assistent message)
        /// </summary>
        public static string ReservedTokensSetting = "SitecoreSuggest.ReservedTokens";

        /// <summary>
        /// The default temperature
        /// </summary>
        public static float DefaultTemperature = 0.5f;

        /// <summary>
        /// The estimated tokens per word
        /// </summary>
        public static int TokensPerWord = 2;

        /// <summary>
        /// The token chars
        /// </summary>
        public static char[] TokenChars = new[] { '.', ',', ';', ':', '!', '-' };

        /// <summary>
        /// The split token chars
        /// </summary>
        public static char[] TokenSplitChars = new[] { ' ', '\'', '.', ',', ';', ':', '!', '-' };

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
        /// The completions model keyword
        /// </summary>
        public static string Completions = "completions";

        /// <summary>
        /// The chat model keyword
        /// </summary>
        public static string Chat = "chat";
    }
}