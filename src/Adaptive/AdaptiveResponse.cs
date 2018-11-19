using System.Collections.Generic;
using System.IO;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;

namespace ShyCharlieBot.Adaptive
{
    public static class AdaptiveResponse
    {
        public static Activity GetAdaptiveResponse(Activity activity, AdaptiveCardType cardType)
        {
            Attachment attachement;
            switch (cardType)
            {
                case AdaptiveCardType.Positive:
                    attachement = CreateAdaptiveCardAttachment(@".\Dialogs\Welcome\Resources\positiveCard.json");
                    break;
                case AdaptiveCardType.Moderate:
                    attachement = CreateAdaptiveCardAttachment(@".\Dialogs\Welcome\Resources\moderateCard.json");
                    break;
                case AdaptiveCardType.Negative:
                    attachement = CreateAdaptiveCardAttachment(@".\Dialogs\Welcome\Resources\negativeCard.json");
                    break;
                default:
                    attachement = CreateAdaptiveCardAttachment(@".\Dialogs\Welcome\Resources\welcomeCard.json");
                    break;
            }

            return CreateResponse(activity, attachement);
        }

        private static Activity CreateResponse(Activity activity, Attachment attachment)
        {
            var response = activity.CreateReply();
            response.Attachments = new List<Attachment> { attachment };
            return response;
        }

        private static Attachment CreateAdaptiveCardAttachment(string cardResource)
        {
            var adaptiveCard = File.ReadAllText(cardResource);
            return BuildAttachement(adaptiveCard);
        }

        private static Attachment BuildAttachement(string adaptiveCard)
        {
            var attachment = new Attachment
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(adaptiveCard)
            };
            return attachment;
        }
    }
}