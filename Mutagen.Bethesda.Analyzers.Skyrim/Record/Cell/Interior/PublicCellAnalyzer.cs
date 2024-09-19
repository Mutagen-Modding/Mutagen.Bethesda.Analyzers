using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Cell.Interior;

public class PublicCellAnalyzer : IIsolatedRecordAnalyzer<ICellGetter>
{
    public static readonly TopicDefinition HasLockList = MutagenTopicBuilder.DevelopmentTopic(
            "Public Cell has Lock List",
            Severity.Warning)
        .WithoutFormatting("Public cell has lock list");

    public IEnumerable<TopicDefinition> Topics { get; } = [HasLockList];

    public RecordAnalyzerResult? AnalyzeRecord(IsolatedRecordAnalyzerParams<ICellGetter> param)
    {
        var cell = param.Record;
        if (cell.IsExteriorCell() || !cell.IsPublic() || cell.LockList.IsNull) return null;

        return new RecordAnalyzerResult(
            RecordTopic.Create(
                cell,
                HasLockList.Format(),
                x => x.Music
            )
        );
    }
}
