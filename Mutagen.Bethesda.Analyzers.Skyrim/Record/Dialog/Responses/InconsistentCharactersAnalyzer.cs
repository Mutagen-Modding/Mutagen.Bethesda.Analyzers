using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
using Noggog;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Dialog.Responses;

public class InconsistentCharactersAnalyzer : IIsolatedRecordAnalyzer<IDialogResponsesGetter>
{
    public static readonly TopicDefinition<string, string> PromptInconsistentCharacters = MutagenTopicBuilder.DevelopmentTopic(
            "Prompt Has Inconsistent Characters",
            Severity.Suggestion)
        .WithFormatting<string, string>("Response {0} contains characters {1} which are not usually used in dialog");

    public static readonly TopicDefinition<string, string> ResponseInconsistentCharacters = MutagenTopicBuilder.DevelopmentTopic(
            "Response Has Inconsistent Characters",
            Severity.Suggestion)
        .WithFormatting<string, string>("Response {0} contains characters {1} which are not usually used in dialog");

    public IEnumerable<TopicDefinition> Topics { get; } = [PromptInconsistentCharacters, ResponseInconsistentCharacters];

    private static readonly char[] InvalidCharacters = ['[', ']'];

    public RecordAnalyzerResult AnalyzeRecord(IsolatedRecordAnalyzerParams<IDialogResponsesGetter> param)
    {
        var dialogResponses = param.Record;

        var result = new RecordAnalyzerResult();

        // Check prompt
        if (dialogResponses.Prompt?.String is not null)
        {
            CheckInconsistentCharacters(dialogResponses.Prompt.String, PromptInconsistentCharacters);
        }

        // Check responses
        foreach (var response in dialogResponses.Responses
                     .Select(x => x.Text.String)
                     .NotNull())
        {
            CheckInconsistentCharacters(response, ResponseInconsistentCharacters);

        }

        return result;

        void CheckInconsistentCharacters(string text, TopicDefinition<string, string> topic)
        {

            var foundCharacters = InvalidCharacters.Where(c => text.Contains(c)).ToArray();
            if (foundCharacters.Length == 0) return;

            result.AddTopic(
                RecordTopic.Create(
                    text,
                    topic.Format(text, string.Join(", ", foundCharacters)),
                    x => x));
        }
    }
}
