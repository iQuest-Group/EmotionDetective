using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using ShyCharlieBot.Adaptive;
using ShyCharlieBot.Dialogs.AnalyserDialog;

namespace ShyCharlieBot.Bot
{
    public class MessageBot : IBot
    {
        private readonly ConversationState _conversationState;
        private readonly ILoggerFactory _loggerFactory;
        private readonly UserState _userState;

        public MessageBot(UserState userState, ConversationState conversationState, ILoggerFactory loggerFactory)
        {
            _userState = userState ?? throw new ArgumentNullException(nameof(userState));
            _conversationState = conversationState ?? throw new ArgumentNullException(nameof(conversationState));
            _loggerFactory = loggerFactory;

            var emotionOutcomeAccessor = _userState.CreateProperty<EmotionOutcome>(nameof(EmotionOutcome));
            var dialogStateAccessor = _conversationState.CreateProperty<DialogState>(nameof(DialogState));

            Dialogs = new DialogSet(dialogStateAccessor);
            Dialogs.Add(new EmotionAnalyserDialog(emotionOutcomeAccessor, loggerFactory));
        }

        private DialogSet Dialogs { get; }

        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellation)
        {
            var activity = turnContext.Activity;
            var context = await Dialogs.CreateContextAsync(turnContext);

            if (activity.Type == ActivityTypes.Message)
            {
                var dialogResult = await context.ContinueDialogAsync();

                if (!context.Context.Responded)
                {
                    if (dialogResult != null && dialogResult.Status == DialogTurnStatus.Empty)
                    {
                        await Start(context, activity);
                    }
                    else if (dialogResult != null)
                    {
                        await Complete(dialogResult, context);
                    }
                }
            }
            else if (IsConversationUpdate(activity))
            {
                foreach (var member in activity.MembersAdded)
                    await SendWelcomeCard(member, activity, context);
            }

            await _conversationState.SaveChangesAsync(turnContext);
            await _userState.SaveChangesAsync(turnContext);
        }

        private static bool IsConversationUpdate(Activity activity)
        {
            return activity.Type == ActivityTypes.ConversationUpdate && activity.MembersAdded.Any();
        }

        private async Task SendWelcomeCard(ChannelAccount member, Activity activity, DialogContext context)
        {
            if (member != null && member.Id != activity.Recipient.Id)
            {
                var response = AdaptiveResponse.GetAdaptiveResponse(activity, AdaptiveCardType.Welcome);
                await context.Context.SendActivityAsync(response).ConfigureAwait(false);
            }
        }

        public async Task Start(DialogContext context, Activity activity)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            await context.BeginDialogAsync(nameof(EmotionAnalyserDialog));

            await context.Context.SendActivityAsync(MessageFactory.Text($"You sent: {activity.Text}.")).ConfigureAwait(false);

            SentimentAnalyser analyser = new SentimentAnalyser(context, activity);

            await analyser.GetMoodScoreFromMS();

            await analyser.GetMoodStateFromDA();
        }

        private async Task Complete( DialogTurnResult dialogResult, DialogContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            switch (dialogResult.Status)
            {
                case DialogTurnStatus.Waiting:
                    // The active dialog is waiting for a response from the user, so do nothing.
                    break;
                case DialogTurnStatus.Complete:
                    await context.EndDialogAsync();
                    break;
                default:
                    await context.CancelAllDialogsAsync();
                    break;
            }
        }
    }
}