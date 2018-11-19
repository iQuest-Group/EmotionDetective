using Newtonsoft.Json;

namespace ShyCharlieBot.TextAnalytics.Microsoft.Models
{
    public class TextRequest
    {
        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}