using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.Analyzers.Skyrim;

public partial class InvalidCharactersAnalyzer : IIsolatedRecordAnalyzer<IDialogResponsesGetter>
{
    public static readonly TopicDefinition<string, string> InvalidCharactersName = MutagenTopicBuilder.FromDiscussion(
            0,
            "Dialog Responses Contains Invalid Characters",
            Severity.Suggestion)
        .WithFormatting<string, string>("Dialog response {0} contain invalid characters: {1}");

    public RecordAnalyzerResult? AnalyzeRecord(IsolatedRecordAnalyzerParams<IDialogResponsesGetter> param)
    {
        var dialogResponses = param.Record;

        var result = new RecordAnalyzerResult();

        foreach (var response in dialogResponses.Responses)
        {
            if (response.Text.String is null) continue;


            var invalidStrings = InvalidStrings.Where(invalidString => response.Text.String.Contains(invalidString.Key)).ToList();
            if (invalidStrings.Count == 0) return null;

            result.AddTopic(
                RecordTopic.Create(
                    dialogResponses,
                    InvalidCharactersName.Format(response.Text.String, string.Join(", ", invalidStrings.Select(x => x.Key))),
                    x => x.Responses
                )
            );
        }

        return result;
    }
}
