using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using ShyCharlieBot.TextAnalytics.DeepAffects.Models;

namespace ShyCharlieBot.TextAnalytics
{
    public class LanguageAnalyticsExtended : Analytics
    {
        protected new const string BaseAddress = "https://proxy.api.deepaffects.com";
        protected new const string SentimentEndpoint = "/text/generic/api/latest/sync/text_recognise_emotion?apikey=AqzNSbBLTpGB0EmKuFC3nt0cVOOrrelF";

        public TextAnalyticsResponse<SentimentResponse> Get(TextToRecognise text)
        {
            var requestContent =
                new StringContent(JsonConvert.SerializeObject(text), Encoding.UTF8, JsonContentType);

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
        }
    }
}