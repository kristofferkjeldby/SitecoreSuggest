namespace SitecoreSuggest
{
    using SitecoreSuggest.Models;
    using System.Collections.Generic;

    /// <summary>
    /// Languages
    /// </summary>
    public static class Languages
    {
        public static Dictionary<string, Language> SupportedLanguages = new Dictionary<string, Language>()
        {
            {
                "en",
                new Language()
                {
                    SummaryPrompt = "Write summary of {0}",
                    WordPrompt = "Use around {0} words",
                    ContextPrompt = "Context: {0}",
                    TokensPerWord = 2.5f
                }
            },
            {
                "da",
                new Language()
                {
                    SummaryPrompt = "Skriv en opsummering af {0}",
                    WordPrompt = "Brug omkring {0} ord",
                    ContextPrompt = "Kontekst: {0}",
                    TokensPerWord = 3
                }
            },
            {
                "fr",
                new Language()
                {
                    SummaryPrompt = "Rédiger un résumé de {0}",
                    WordPrompt = "Utilisez environ {0} mots",
                    ContextPrompt = "Contexte: {0}",
                    TokensPerWord = 3
                }
            }
        };
    }
}