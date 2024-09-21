using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Dialog.Responses;

public class CircularPreviousDialogueAnalyzer : IContextualRecordAnalyzer<IDialogResponsesGetter>
{
    public static readonly TopicDefinition<IDialogResponsesGetter, IDialogResponsesGetter> CircularPreviousDialogue = MutagenTopicBuilder.DevelopmentTopic(
            "Circular Previous Dialog",
            Severity.Warning)
        .WithFormatting<IDialogResponsesGetter, IDialogResponsesGetter>("Dialogue has a circular reference in the previous dialog between {0} and {1}");

    public IEnumerable<TopicDefinition> Topics { get; } = [CircularPreviousDialogue];

    public RecordAnalyzerResult? AnalyzeRecord(ContextualRecordAnalyzerParams<IDialogResponsesGetter> param)
    {
        var dialogResponses = param.Record;
        var dialogCache = new HashSet<FormKey>();

        var previousDialog = dialogResponses.PreviousDialog.TryResolve(param.LinkCache);
        while (previousDialog?.PreviousDialog is not null)
        {
            if (!dialogCache.Add(previousDialog.FormKey))
            {
                return new RecordAnalyzerResult(
                    RecordTopic.Create(
                        dialogResponses,
                        CircularPreviousDialogue.Format(dialogResponses, previousDialog),
                        x => x.PreviousDialog
                    )
                );
            }

            previousDialog = previousDialog.PreviousDialog.TryResolve(param.LinkCache);
        }

        return null;
    }
}
