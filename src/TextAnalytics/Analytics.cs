using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace ShyCharlieBot.TextAnalytics
{
    public abstract class Analytics
    {
        protected HttpClient HttpClient;
        protected const string JsonContentType = "application/json";
        protected const string BaseAddress = "";
        protected const string SentimentEndpoint = "";

        protected Analytics()
        {
            if (HttpClient == null)
            {
                SetUpHttpClient();
            }
        }

        public abstract void SetUpHttpClient();

        protected TextAnalyticsResponse<T> ReadResponseMessage<T>(HttpResponseMessage responseMessage)
        {
            if (responseMessage.IsSuccessStatusCode)
            {
                string content = responseMessage.Content.ReadAsStringAsync().Result;

                T result = JsonConvert.DeserializeObject<T>(content);

                return new TextAnalyticsResponse<T>(result, responseMessage.StatusCode);
            }

            return new TextAnalyticsResponse<T>(default(T), responseMessage.StatusCode);
        }

        public class TextAnalyticsResponse<T> : Response
        {
            public T Content { get; }

            public TextAnalyticsResponse(T content, HttpStatusCode statusCode)
                : base(statusCode)
            {
                Content = content;
            }
        }

        public class Response
        {
            public HttpStatusCode StatusCode { get; }

            public Response(HttpStatusCode statusCode)
            {
                StatusCode = statusCode;
            }

            public bool IsSuccess()
            {
                return StatusCode == HttpStatusCode.OK;
            }
        }
    }
}