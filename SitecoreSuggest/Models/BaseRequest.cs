﻿
namespace SitecoreSuggest.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// Represent a base request 
    /// </summary>
    public abstract class BaseRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChatRequest"/> class.
        /// </summary>
        public BaseRequest()
        {
            N = 1;
        }

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