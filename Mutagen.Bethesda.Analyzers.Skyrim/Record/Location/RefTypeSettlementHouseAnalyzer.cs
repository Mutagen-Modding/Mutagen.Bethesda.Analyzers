using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Location;

public class RefTypeSettlementHouseAnalyzer : IContextualRecordAnalyzer<ILocationGetter>
{
    public static readonly TopicDefinition NoHouseContainerRefType = MutagenTopicBuilder.DevelopmentTopic(
            "No House Container Ref Type",
            Severity.Suggestion)
        .WithoutFormatting("Settlement house location has no House Container Ref Type - this is likely a mistake");

    public IEnumerable<TopicDefinition> Topics { get; } = [NoHouseContainerRefType];

    private static readonly List<IFormLinkGetter<ILocationReferenceTypeGetter>> HouseContainerRefTypes =
    [
        FormKeys.SkyrimSE.Skyrim.LocationReferenceType.HouseContainerRefType,
        FormKeys.SkyrimSE.Skyrim.LocationReferenceType.TGRWealthMarker01,
        FormKeys.SkyrimSE.Skyrim.LocationReferenceType.TGRWealthMarker02,
        FormKeys.SkyrimSE.Skyrim.LocationReferenceType.TGRWealthMarker03,
        FormKeys.SkyrimSE.Skyrim.LocationReferenceType.TGRWealthyHomeChest,
        FormKeys.SkyrimSE.Skyrim.LocationReferenceType.TGRLedger,
        FormKeys.SkyrimSE.Skyrim.LocationReferenceType.TGRSLStrongbox
    ];

    public RecordAnalyzerResult? AnalyzeRecord(ContextualRecordAnalyzerParams<ILocationGetter> param)
    {
        var location = param.Record;

        if (!location.IsSettlementLocation()) return null;
        if (!location.IsSettlementHouseLocationNotPlayerHome()) return null;
        if (!location.IsLocationAppliedToInterior(param.LinkCache)) return null;

        if (location.GetReferenceTypes().Any(staticRef => HouseContainerRefTypes.Contains(staticRef.LocationRefType)))
        {
            return null;
        }

        return new RecordAnalyzerResult(
            RecordTopic.Create(
                location,
                NoHouseContainerRefType.Format(),
                x => x.LocationCellStaticReferences));
    }
}
