using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Plugins.Aspects;
using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.Analyzers.Skyrim;

public partial class InvalidCharactersAnalyzer : IIsolatedRecordAnalyzer<ISkyrimMajorRecordGetter>
{
    public static readonly TopicDefinition<string> InvalidCharactersName = MutagenTopicBuilder.FromDiscussion(
            0,
            "Invalid Characters in Name",
            Severity.Warning)
        .WithFormatting<string>("The name contains invalid characters: {0}");

    public RecordAnalyzerResult? AnalyzeRecord(IsolatedRecordAnalyzerParams<ISkyrimMajorRecordGetter> param)
    {
        if (param.Record is not INamedGetter { Name: not null } named) return null;

        var invalidStrings = InvalidStrings.Where(invalidString => named.Name.Contains(invalidString.Key)).ToList();
        if (invalidStrings.Count == 0) return null;

        return new RecordAnalyzerResult(
            RecordTopic.Create(
                named,
                InvalidCharactersBookText.Format(string.Join(", ", invalidStrings.Select(x => x.Key))),
                x => x.Name
            )
        );
    }
}
