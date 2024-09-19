using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.Analyzers.Skyrim;

public partial class InvalidCharactersAnalyzer : IIsolatedRecordAnalyzer<IBookGetter>
{
    public static readonly TopicDefinition<string> InvalidCharactersBookText = MutagenTopicBuilder.DevelopmentTopic(
            "Book Text Contains Invalid Characters",
            Severity.Warning)
        .WithFormatting<string>("Book text contains invalid characters: {0}");

    public RecordAnalyzerResult? AnalyzeRecord(IsolatedRecordAnalyzerParams<IBookGetter> param)
    {
        var book = param.Record;
        if (book.BookText.String is null) return null;

        var invalidStrings = InvalidStrings.Where(invalidString => book.BookText.String.Contains(invalidString.Key)).ToList();
        if (invalidStrings.Count == 0) return null;

        return new RecordAnalyzerResult(
            RecordTopic.Create(
                book,
                InvalidCharactersBookText.Format(string.Join(", ", invalidStrings.Select(x => x.Key))),
                x => x.BookText
            )
        );
    }
}
