using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShyCharlieBot.TextAnalytics.Microsoft.Models
{
    public class TextMessages
    {
        [JsonProperty("documents")]
        public List<TextRequest> Messages { get; set; }
    }
}