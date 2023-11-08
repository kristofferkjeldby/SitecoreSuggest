
namespace SitecoreSuggest.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// Represent a completion request 
    /// </summary>
    public class CompletionsRequest : BaseRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompletionsRequest"/> class.
        /// </summary>
        public CompletionsRequest() : base()
        {

        }

        /// <summary>
        /// Gets or sets the prompt.
        /// </summary>
        [JsonProperty("prompt")]
        public string Prompt { get; set; }
    }

}