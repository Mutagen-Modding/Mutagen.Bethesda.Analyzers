using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Cell.Interior.Settlement;

public class NoOwnerAnalyzer : IContextualRecordAnalyzer<ICellGetter>
{
    public static readonly TopicDefinition NoOwner = MutagenTopicBuilder.DevelopmentTopic(
            "No Owner",
            Severity.Suggestion)
        .WithoutFormatting("Cell has no owner");

    public IEnumerable<TopicDefinition> Topics { get; } = [NoOwner];

    public RecordAnalyzerResult AnalyzeRecord(ContextualRecordAnalyzerParams<ICellGetter> param)
    {
        var cell = param.Record;
        var result = new RecordAnalyzerResult();

        if (!cell.IsSettlementCell(param.LinkCache)) return result;

        var location = cell.Location.TryResolve(param.LinkCache);
        if (location is null) return result;
        if (location.HasKeyword(FormKeys.SkyrimSE.Skyrim.Keyword.LocTypePlayerHouse)) return result;

        if (cell.Owner.IsNull)
        {
            result.AddTopic(
                RecordTopic.Create(
                    cell,
                    NoOwner.Format(),
                    x => x.Owner
                )
            );
        }

        return result;
    }
}
