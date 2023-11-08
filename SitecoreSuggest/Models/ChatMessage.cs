namespace SitecoreSuggest.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a chat request message
    /// </summary>
    public class ChatMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChatMessage"/> class.
        /// </summary>
        public ChatMessage(string role, string content)
        {
            Role = role;
            Content = content;
        }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        [JsonProperty("role")]
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }
    }
}