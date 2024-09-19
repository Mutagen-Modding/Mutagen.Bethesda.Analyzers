using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
using Noggog;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Dialog.Topic;

public class TooLongAnalyzer : IIsolatedRecordAnalyzer<IDialogTopicGetter>
{
    private const int DialogPromptLengthLimit = 80;
    public static readonly TopicDefinition<string, int> TopicPromptTooLong = MutagenTopicBuilder.DevelopmentTopic(
            "Topic Prompt Too Long",
            Severity.Suggestion)
        .WithFormatting<string, int>("Topic prompt '{0}' is {1} longer than the recommended limit " + DialogPromptLengthLimit);

    public IEnumerable<TopicDefinition> Topics { get; } = [TopicPromptTooLong];

    public RecordAnalyzerResult? AnalyzeRecord(IsolatedRecordAnalyzerParams<IDialogTopicGetter> param)
    {
        var dialogTopic = param.Record;

        if (dialogTopic.Name?.String is { Length: > DialogPromptLengthLimit })
        {
            return new RecordAnalyzerResult(
                RecordTopic.Create(
                    dialogTopic,
                    TopicPromptTooLong.Format(dialogTopic.Name.String, dialogTopic.Name.String.Length - DialogPromptLengthLimit),
                    x => x.Name
                )
            );
        }

        return null;
    }
}
