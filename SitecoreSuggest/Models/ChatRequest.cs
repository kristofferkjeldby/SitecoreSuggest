
namespace SitecoreSuggest.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// Represent a chat request 
    /// </summary>
    public class ChatRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChatRequest"/> class.
        /// </summary>
        public ChatRequest()
        {
            Messages = new List<ChatMessage>();
            N = 1;
        }

        /// <summary>
        /// Gets or sets the messages.
        /// </summary>
        [JsonProperty("messages")]
        public IList<ChatMessage> Messages { get; set; }

        /// <summary>
        /// Gets or sets the maximum tokens.
        /// </summary>
        [JsonProperty("max_tokens")]
        public int MaxTokens { get; set; }

        /// <summary>
        /// Gets or sets the n.
        /// </summary>
        [JsonProperty("n")]
        public int N {  get; set; }

        /// <summary>
        /// Gets or sets the stop.
        /// </summary>
        [JsonProperty("stop")]
        public string Stop { get; set; }

        /// <summary>
        /// Gets or sets the temperature.
        /// </summary>
        [JsonProperty("temperature")]
        public float Temperature { get; set; }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        [JsonProperty("model")]
        public string Model { get; set; }
    }

}