using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Extensions.Logging;

namespace ShyCharlieBot.Dialogs.AnalyserDialog
{
    public class EmotionAnalyserDialog : ComponentDialog
    {
        private const string EmotionInspectorDialog = "emotionDialog";

        public EmotionAnalyserDialog(IStatePropertyAccessor<EmotionOutcome> userProfileStateAccessor, ILoggerFactory loggerFactory)
            : base(nameof(EmotionAnalyserDialog))
        {
            UserProfileAccessor = userProfileStateAccessor ?? throw new ArgumentNullException(nameof(userProfileStateAccessor));

            var waterfallSteps = new WaterfallStep[] { InitializeStateStepAsync, GiveFeedback };

            AddDialog(new WaterfallDialog(EmotionInspectorDialog, waterfallSteps));
         }

        public IStatePropertyAccessor<EmotionOutcome> UserProfileAccessor { get; }

        private async Task<DialogTurnResult> InitializeStateStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var dialogState = await UserProfileAccessor.GetAsync(stepContext.Context, () => null, cancellationToken);
            if (dialogState != null)
            {
                return await stepContext.NextAsync(cancellationToken: cancellationToken);
            }

            if (stepContext.Options is EmotionOutcome outcome)
            {
                await UserProfileAccessor.SetAsync(stepContext.Context, outcome, cancellationToken);
            }
            else
            {
                await UserProfileAccessor.SetAsync(stepContext.Context, new EmotionOutcome(), cancellationToken);
            }

            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> GiveFeedback(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            string message = (string)stepContext.Result;

            if (!string.IsNullOrEmpty(message))
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Pleased to meet you, {message}."),
                    cancellationToken);

                var context = stepContext.Context;
                var emotionOutcome = await UserProfileAccessor.GetAsync(context, cancellationToken: cancellationToken);
                emotionOutcome.Received = message;

                await context.SendActivityAsync(emotionOutcome.Received, cancellationToken: cancellationToken);
            }

            return await stepContext.ContinueDialogAsync(cancellationToken);
        }
    }
}
