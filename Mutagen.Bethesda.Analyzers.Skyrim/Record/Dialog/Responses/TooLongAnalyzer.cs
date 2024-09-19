using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
using Noggog;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Dialog.Responses;

public class TooLongAnalyzer : IIsolatedRecordAnalyzer<IDialogResponsesGetter>
{
    private const int DialogPromptLengthLimit = 80;
    private const int DialogResponseLengthLimit = 149;

    public static readonly TopicDefinition<string, int> PromptTooLong = MutagenTopicBuilder.FromDiscussion(
            0,
            "Prompt Too Long",
            Severity.Suggestion)
        .WithFormatting<string, int>("Prompt '{0}' is {1} longer than the recommended limit " + DialogPromptLengthLimit);

    public static readonly TopicDefinition<string, int> ResponseTooLong = MutagenTopicBuilder.FromDiscussion(
            0,
            "Response Too Long",
            Severity.Suggestion)
        .WithFormatting<string, int>("Response '{0}' is {1} longer than the recommended limit " + DialogResponseLengthLimit);

    public IEnumerable<TopicDefinition> Topics { get; } = [PromptTooLong, ResponseTooLong];

    public RecordAnalyzerResult AnalyzeRecord(IsolatedRecordAnalyzerParams<IDialogResponsesGetter> param)
    {
        var dialogResponses = param.Record;


        var result = new RecordAnalyzerResult();

        // Check prompt
        if (dialogResponses.Prompt?.String is { Length: > DialogPromptLengthLimit })
        {
            result.AddTopic(
                RecordTopic.Create(
                    dialogResponses,
                    PromptTooLong.Format(dialogResponses.Prompt.String, dialogResponses.Prompt.String.Length - DialogPromptLengthLimit),
                    x => x));
        }

        // Check responses
        foreach (var response in dialogResponses.Responses
                     .Select(x => x.Text.String)
                     .NotNull()
                     .Where(text => text is { Length: > DialogResponseLengthLimit }))
        {
            result.AddTopic(
                RecordTopic.Create(
                    dialogResponses,
                    ResponseTooLong.Format(response, response.Length - DialogResponseLengthLimit),
                    x => x));
        }

        return result;
    }
}
