using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Cell.Interior.Settlement;

public class NoLockListAnalyzer : IContextualRecordAnalyzer<ICellGetter>
{
    public static readonly TopicDefinition NoLockList = MutagenTopicBuilder.DevelopmentTopic(
            "No Lock List",
            Severity.Suggestion)
        .WithoutFormatting("Cell has no lock list");

    public IEnumerable<TopicDefinition> Topics { get; } = [NoLockList];

    public RecordAnalyzerResult AnalyzeRecord(ContextualRecordAnalyzerParams<ICellGetter> param)
    {
        var cell = param.Record;
        var result = new RecordAnalyzerResult();

        // Public cells should not have a lock list
        if (cell.IsPublic()) return result;

        if (cell.LockList.IsNull)
        {
            result.AddTopic(
                RecordTopic.Create(
                    cell,
                    NoLockList.Format(),
                    x => x.Owner
                )
            );
        }

        return result;
    }
}
