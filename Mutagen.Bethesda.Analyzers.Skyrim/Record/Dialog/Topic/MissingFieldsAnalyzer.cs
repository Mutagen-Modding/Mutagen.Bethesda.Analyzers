using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Dialog.Topic;

public class MissingFieldsAnalyzer : IIsolatedRecordAnalyzer<IDialogTopicGetter>
{
    public static readonly TopicDefinition NoBranch = MutagenTopicBuilder.DevelopmentTopic(
            "No Branch",
            Severity.Error)
        .WithoutFormatting("Topic has no branch, it will not be available in game");

    public static readonly TopicDefinition NoQuest = MutagenTopicBuilder.DevelopmentTopic(
            "No Quest",
            Severity.Error)
        .WithoutFormatting("Topic has no quest, it will not be available in game");

    public IEnumerable<TopicDefinition> Topics { get; } = [NoBranch, NoQuest];

    public RecordAnalyzerResult? AnalyzeRecord(IsolatedRecordAnalyzerParams<IDialogTopicGetter> param)
    {
        var dialogTopic = param.Record;

        var result = new RecordAnalyzerResult();

        if (dialogTopic.Branch.IsNull)
        {
            result.AddTopic(
                RecordTopic.Create(
                    dialogTopic,
                    NoBranch.Format(),
                    x => x.Branch
                )
            );
        }


        if (dialogTopic.Quest.IsNull)
        {
            result.AddTopic(
                RecordTopic.Create(
                    dialogTopic,
                    NoQuest.Format(),
                    x => x.Quest
                )
            );
        }

        return result;
    }
}
