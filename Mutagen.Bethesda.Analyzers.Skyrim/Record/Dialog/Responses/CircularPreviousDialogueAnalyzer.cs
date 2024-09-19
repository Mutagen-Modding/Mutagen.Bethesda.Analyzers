using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Dialog.Responses;

public class CircularPreviousDialogueAnalyzer : IContextualRecordAnalyzer<IDialogResponsesGetter>
{
    public static readonly TopicDefinition<string, string> CircularPreviousDialogue = MutagenTopicBuilder.FromDiscussion(
            0,
            "Circular Previous Dialog",
            Severity.Warning)
        .WithFormatting<string, string>("Dialogue has a circular reference in the previous dialog between {0} and {1}");

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
                        CircularPreviousDialogue.Format(
                            dialogResponses.FormKey.ToString(),
                            previousDialog.FormKey.ToString()
                        ),
                        x => x.PreviousDialog
                    )
                );
            }

            previousDialog = previousDialog.PreviousDialog.TryResolve(param.LinkCache);
        }

        return null;
    }
}
