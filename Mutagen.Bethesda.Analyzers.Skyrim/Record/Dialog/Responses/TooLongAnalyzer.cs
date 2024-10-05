using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
using Noggog;

namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Dialog.Responses;

public class TooLongAnalyzer : IIsolatedRecordAnalyzer<IDialogResponsesGetter>
{
    private const int DialogPromptLengthLimit = 80;
    private const int DialogResponseLengthLimit = 149;

    public static readonly TopicDefinition<string, int> PromptTooLong = MutagenTopicBuilder.DevelopmentTopic(
            "Prompt Too Long",
            Severity.Suggestion)
        .WithFormatting<string, int>("Prompt '{0}' is {1} longer than the recommended limit " + DialogPromptLengthLimit);

    public static readonly TopicDefinition<string, int> ResponseTooLong = MutagenTopicBuilder.DevelopmentTopic(
            "Response Too Long",
            Severity.Suggestion)
        .WithFormatting<string, int>("Response '{0}' is {1} longer than the recommended limit " + DialogResponseLengthLimit);

    public IEnumerable<TopicDefinition> Topics { get; } = [PromptTooLong, ResponseTooLong];

    public void AnalyzeRecord(IsolatedRecordAnalyzerParams<IDialogResponsesGetter> param)
    {
        var dialogResponses = param.Record;

        // Check prompt
        if (dialogResponses.Prompt?.String is { Length: > DialogPromptLengthLimit })
        {
            param.AddTopic(
                PromptTooLong.Format(dialogResponses.Prompt.String, dialogResponses.Prompt.String.Length - DialogPromptLengthLimit),
                x => x);
        }

        // Check responses
        foreach (var response in dialogResponses.Responses
                     .Select(x => x.Text.String)
                     .NotNull()
                     .Where(text => text is { Length: > DialogResponseLengthLimit }))
        {
            param.AddTopic(
                ResponseTooLong.Format(response, response.Length - DialogResponseLengthLimit),
                x => x);
        }
    }
}
