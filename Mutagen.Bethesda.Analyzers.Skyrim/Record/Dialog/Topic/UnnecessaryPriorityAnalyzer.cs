using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
using Noggog;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Dialog.Topic;

public class UnnecessaryPriorityAnalyzer : IContextualRecordAnalyzer<IDialogTopicGetter>
{
    private const int DefaultDialogTopicPriority = 50;

    public static readonly TopicDefinition<float> UnnecessaryPriority = MutagenTopicBuilder.DevelopmentTopic(
            "Unnecessary Priority",
            Severity.Suggestion)
        .WithFormatting<float>("Topic has a custom priority of {0} but is not a starting topic, this does not have any effect on the dialog order");

    public IEnumerable<TopicDefinition> Topics { get; } = [UnnecessaryPriority];

    public RecordAnalyzerResult? AnalyzeRecord(ContextualRecordAnalyzerParams<IDialogTopicGetter> param)
    {
        var dialogTopic = param.Record;

        // Only check custom topics
        if (dialogTopic.Subtype != DialogTopic.SubtypeEnum.Custom) return null;

        // Skip if this topic is a starting topic
        var branch = dialogTopic.Branch.TryResolve(param.LinkCache);
        if (branch is not null && branch.StartingTopic.FormKey == dialogTopic.FormKey) return null;

        if (Math.Abs(dialogTopic.Priority - DefaultDialogTopicPriority) < float.Epsilon)
        {
            return null;
        }

        return new RecordAnalyzerResult(
            RecordTopic.Create(
                dialogTopic,
                UnnecessaryPriority.Format(dialogTopic.Priority),
                x => x.Priority
            )
        );
    }
}
