namespace SitecoreSuggest
{
    using SitecoreSuggest.Models;
    using System.Collections.Generic;

    /// <summary>
    /// Languages
    /// </summary>
    public static class Languages
    {
        public static Dictionary<string, Prompts> SupportedLanguages = new Dictionary<string, Prompts>()
        {
            {
                "en",
                new Prompts()
                {
                    SummaryPrompt = "Write summary of \"{0}\"",
                    WordPrompt = "Use around {0} words"
                }
            },
            {
                "da",
                new Prompts()
                {
                    SummaryPrompt = "Skriv en opsummering om \"{0}\"",
                    WordPrompt = "Brug omkring {0} ord"
                }
            }
        };
    }
}