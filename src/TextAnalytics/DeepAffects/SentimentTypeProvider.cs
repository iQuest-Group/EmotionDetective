using System;
using System.Reflection;

namespace ShyCharlieBot.TextAnalytics.DeepAffects.Models
{
    public static class SentimentTypeProvider
    {
        public static string GetSentimentType(SentimentTypeResponse response)
        {
            Type responseType = response.GetType();
            PropertyInfo[] pi = responseType.GetProperties();

            foreach (PropertyInfo property in pi)
            {
                if ((double)property.GetValue(response) == 1.0)
                {
                    return property.Name;
                }
            }

            return "Nothing";
        }
    }
}