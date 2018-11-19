using Newtonsoft.Json;

namespace ShyCharlieBot.TextAnalytics.DeepAffects.Models
{
    public class SentimentTypeResponse
    {
        [JsonProperty("anticipation")]
        public double Anticipation { get; set; }

        [JsonProperty("joy")]
        public double Joy { get; set; }

        [JsonProperty("sadness")]
        public double Sadness { get; set; }

        [JsonProperty("neutral")]
        public double Neutral { get; set; }

        [JsonProperty("disgust")]
        public double Disgust { get; set; }

        [JsonProperty("anger")]
        public double Anger { get; set; }

        [JsonProperty("surprise")]
        public double Surprise { get; set; }

        [JsonProperty("fear")]
        public double Fear { get; set; }

        [JsonProperty("trust")]
        public double Trust { get; set; }
    }
}