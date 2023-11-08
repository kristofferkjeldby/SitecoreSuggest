namespace SitecoreSuggest.Models
{
    /// <summary>
    /// Language prompts
    /// </summary>
    public class LanguagePrompts
    {
        /// <summary>
        /// Gets or sets the summary prompt.
        /// </summary>
        public string SummaryPrompt {get; set; }

        /// <summary>
        /// Gets or sets the word prompt.
        /// </summary>
        public string WordPrompt { get; set; }

        /// <summary>
        /// Gets or sets the context prompt.
        /// </summary>
        public string ContextPrompt { get; set; }
    }
}