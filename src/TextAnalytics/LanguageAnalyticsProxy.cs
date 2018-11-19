using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using ShyCharlieBot.TextAnalytics.Microsoft.Models;

namespace ShyCharlieBot.TextAnalytics
{
    public class LanguageAnalyticsProxy : Analytics
    {
        protected new const string BaseAddress = "https://westeurope.api.cognitive.microsoft.com";
        protected new const string SentimentEndpoint = "/text/analytics/v2.0/sentiment";

        public TextAnalyticsResponse<SentimentResponse> Get(TextMessages messages)
        {
            var requestContent =
                new StringContent(JsonConvert.SerializeObject(messages), Encoding.UTF8, JsonContentType);

            var httpResponse = HttpClient.PostAsync(SentimentEndpoint, requestContent).Result;
            httpResponse.EnsureSuccessStatusCode();

            TextAnalyticsResponse<SentimentResponse> response = ReadResponseMessage<SentimentResponse>(httpResponse);

            return response;
        }

        public override void SetUpHttpClient()
        {
            HttpClient = new HttpClient
            {
                BaseAddress = new Uri(BaseAddress)
            };
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(JsonContentType));
            HttpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "b36ebb5303cf489c9d3900b04d9cb0d8");
        }
    }
}