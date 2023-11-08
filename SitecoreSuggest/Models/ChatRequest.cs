
namespace SitecoreSuggest.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// Represent a chat request 
    /// </summary>
    public class ChatRequest : BaseRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChatRequest"/> class.
        /// </summary>
        public ChatRequest() : base()
        {
            Messages = new List<ChatMessage>();
        }

        /// <summary>
        /// Gets or sets the messages.
        /// </summary>
        [JsonProperty("messages")]
        public IList<ChatMessage> Messages { get; set; }
    }
}