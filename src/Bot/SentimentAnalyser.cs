using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using ShyCharlieBot.TextAnalytics;
using ShyCharlieBot.TextAnalytics.DeepAffects.Models;
using ShyCharlieBot.TextAnalytics.Microsoft.Models;

namespace ShyCharlieBot.Bot
{
    public class SentimentAnalyser
    {
        private readonly DialogContext _dialogContext;
        private readonly Activity _activity;

        public SentimentAnalyser(DialogContext context, Activity activity)
        {
            _dialogContext = context;
            _activity = activity;
        }

        public async Task GetMoodScoreFromMS()
        {
            var messages = BuildTextMessage(_activity.Text);
            var languageAnalytics = new LanguageAnalyticsProxy();
            var score = languageAnalytics.Get(messages).Content.Sentiments.First().Score;

            await _dialogContext.Context.SendActivityAsync(MessageFactory.Text($"Score: {score}"))
                .ConfigureAwait(false);
        }

        public async Task GetMoodStateFromDA()
        {
            var daLanguageAnalytics = new LanguageAnalyticsExtended();
            var sentiment = daLanguageAnalytics.Get(BuildDeepAffects(_activity.Text));
            var sentimentType = SentimentTypeProvider.GetSentimentType(sentiment.Content.Response);

            await _dialogContext.Context.SendActivityAsync(MessageFactory.Text($"Your sentiment is: {sentimentType}."))
                .ConfigureAwait(false);
        }

        private static TextMessages BuildTextMessage(string message)
        {
            var messages = new TextMessages
            {
                Messages = new List<TextRequest> {new TextRequest {Id = "1", Language = "en", Text = message}}
            };
            return messages;
        }

        private static TextToRecognise BuildDeepAffects(string message)
        {
            var text = new TextToRecognise
            {
                Content = message
            };
            return text;
        }
    }
}