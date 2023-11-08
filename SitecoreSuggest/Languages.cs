namespace SitecoreSuggest
{
    using SitecoreSuggest.Models;
    using System.Collections.Generic;

    /// <summary>
    /// Languages
    /// </summary>
    public static class Languages
    {
        public static Dictionary<string, LanguagePrompts> SupportedLanguages = new Dictionary<string, LanguagePrompts>()
        {
            {
                "en",
                new LanguagePrompts()
                {
                    SummaryPrompt = "Write summary of \"{0}\"",
                    WordPrompt = "Use around {0} words",
                    ContextPrompt = "Context: {0}"
                }
            },
            {
                "da",
                new LanguagePrompts()
                {
                    SummaryPrompt = "Skriv en opsummering om \"{0}\"",
                    WordPrompt = "Brug omkring {0} ord",
                    ContextPrompt = "Kontekst: {0}"
                }
            }
        };
    }
}