using Newtonsoft.Json;

namespace ShyCharlieBot.TextAnalytics.DeepAffects.Models
{
    public class TextToRecognise
    {
        [JsonProperty("content")]
        public string Content { get; set; }
    }
}
