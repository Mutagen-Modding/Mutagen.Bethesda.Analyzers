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

    public void AnalyzeRecord(ContextualRecordAnalyzerParams<ICellGetter> param)
    {
        var cell = param.Record;

        if (!cell.IsSettlementCell(param.LinkCache)) return;

        var location = cell.Location.TryResolve(param.LinkCache);
        if (location is null) return;
        if (location.HasKeyword(FormKeys.SkyrimSE.Skyrim.Keyword.LocTypePlayerHouse)) return;

        if (cell.Owner.IsNull)
        {
            param.AddTopic(
                NoOwner.Format(),
                x => x.Owner);
        }
    }
}
