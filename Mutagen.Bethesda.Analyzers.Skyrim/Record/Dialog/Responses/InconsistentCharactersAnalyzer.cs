using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
using Noggog;

namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Dialog.Responses;

public class InconsistentCharactersAnalyzer : IIsolatedRecordAnalyzer<IDialogResponsesGetter>
{
    public static readonly TopicDefinition<string, char[]> PromptInconsistentCharacters = MutagenTopicBuilder.DevelopmentTopic(
            "Prompt Has Inconsistent Characters",
            Severity.Suggestion)
        .WithFormatting<string, char[]>("Response {0} contains characters {1} which are not usually used in dialog");

    public static readonly TopicDefinition<string, char[]> ResponseInconsistentCharacters = MutagenTopicBuilder.DevelopmentTopic(
            "Response Has Inconsistent Characters",
            Severity.Suggestion)
        .WithFormatting<string, char[]>("Response {0} contains characters {1} which are not usually used in dialog");

    public IEnumerable<TopicDefinition> Topics { get; } = [PromptInconsistentCharacters, ResponseInconsistentCharacters];

    private static readonly char[] InvalidCharacters = ['[', ']'];

    public void AnalyzeRecord(IsolatedRecordAnalyzerParams<IDialogResponsesGetter> param)
    {
        var dialogResponses = param.Record;

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

        return;

        void CheckInconsistentCharacters(string text, TopicDefinition<string, char[]> topic)
        {
            var foundCharacters = InvalidCharacters.Where(text.Contains).ToArray();
            if (foundCharacters.Length == 0) return;

            param.AddTopic(
                topic.Format(text, foundCharacters));
        }
    }

    public IEnumerable<Func<IDialogResponsesGetter, object?>> FieldsOfInterest()
    {
        yield return x => x.Prompt;
        yield return x => x.Responses;
    }
}
