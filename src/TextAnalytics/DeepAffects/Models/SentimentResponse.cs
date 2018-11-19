using Newtonsoft.Json;

namespace ShyCharlieBot.TextAnalytics.DeepAffects.Models
{
    public class SentimentResponse
    {

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("response")]
        public SentimentTypeResponse Response { get; set; }
    }
}
