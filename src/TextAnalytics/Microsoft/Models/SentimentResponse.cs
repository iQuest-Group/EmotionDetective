using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShyCharlieBot.TextAnalytics.Microsoft.Models
{
    public class SentimentResponse
    {
        [JsonProperty("documents")]
        public List<Sentiment> Sentiments { get; set; }

        [JsonProperty("errors")]
        public List<Error> Errors { get; set; }
    }
}