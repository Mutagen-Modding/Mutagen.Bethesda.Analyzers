using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
using Noggog;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Dialog.Responses;

public class NotTrimmedAnalyzer : IIsolatedRecordAnalyzer<IDialogResponsesGetter>
{
    public static readonly TopicDefinition<string> PromptNotTrimmed = MutagenTopicBuilder.FromDiscussion(
            0,
            "Prompt Not Trimmed",
            Severity.Suggestion)
        .WithFormatting<string>("Prompt '{0}' is not trimmed");

    public static readonly TopicDefinition<string> ResponseNotTrimmed = MutagenTopicBuilder.FromDiscussion(
            0,
            "Response Not Trimmed",
            Severity.Suggestion)
        .WithFormatting<string>("Response '{0}' is not trimmed");

    public IEnumerable<TopicDefinition> Topics { get; } = [PromptNotTrimmed, ResponseNotTrimmed];

    public RecordAnalyzerResult AnalyzeRecord(IsolatedRecordAnalyzerParams<IDialogResponsesGetter> param)
    {
        var dialogResponses = param.Record;
        var result = new RecordAnalyzerResult();

        // Check prompt
        if (dialogResponses.Prompt?.String != null && NotTrimmed(dialogResponses.Prompt.String))
        {
            result.AddTopic(
                RecordTopic.Create(
                    dialogResponses,
                    PromptNotTrimmed.Format(dialogResponses.Prompt.String),
                    x => x));
        }

        // Check responses
        foreach (var response in dialogResponses.Responses.Select(dialogResponse => dialogResponse.Text.String)
                     .NotNull()
                     .Where(text => !text.IsNullOrWhitespace())
                     .Where(NotTrimmed)
                )
        {
            result.AddTopic(
                RecordTopic.Create(
                    dialogResponses,
                    ResponseNotTrimmed.Format(response),
                    x => x));
        }

        return result;

        static bool NotTrimmed(string text) => text.StartsWith(' ') || text.EndsWith(' ');
    }
}
