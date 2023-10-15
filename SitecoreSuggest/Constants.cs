using System.Collections.Generic;

namespace SitecoreSuggest
{
    public static class Constants
    {
        public static string[] SupportedTypeKeys = new string[] {
            "multi-line text",
            "single-line text",
            "rich text"
        };

        public static string[] SummaryFields = new string[] {
            "single-line text",
        };

        public static Dictionary<string, string> SummaryStrings = new Dictionary<string, string>
        {
            { "en", "Write summary of \"{0}\"" },
            { "da", "Skriv en opsummering om \"{0}\""}
        };

        public static string[] HtmlTypeKeys = new string[] {
            "rich text"
        };

        public static string ApiKeySetting = "SitecoreSuggest.ApiKey";

        public static string EndpointSetting = "SitecoreSuggest.Endpoint";

        public static string ModelSetting = "SitecoreSuggest.Model";

        public static int DefaultWords = 2000;

        public static float WordsPerToken = 0.75f;

        public static int ShortValueLength = 70;

        public static string Insert = "insert";

        public static string Append = "append";
    }
}