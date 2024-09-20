using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Placed.Object;

public class MineOreAnalyzer : IContextualRecordAnalyzer<IPlacedObjectGetter>
{
    public static readonly TopicDefinition NoFurnitureLinked = MutagenTopicBuilder.DevelopmentTopic(
            "No Furniture Linked",
            Severity.Error)
        .WithoutFormatting("Mine Ore has no furniture linked");

    public IEnumerable<TopicDefinition> Topics { get; } = [NoFurnitureLinked];

    private static readonly HashSet<IFormLinkGetter<IPlaceableObjectGetter>> MiningFurniture =
    [
        FormKeys.SkyrimSE.Skyrim.Furniture.PickaxeMiningTableMarker,
        FormKeys.SkyrimSE.Skyrim.Furniture.PickaxeMiningFloorMarker,
        FormKeys.SkyrimSE.Skyrim.Furniture.PickaxeMiningWallMarker,
        FormKeys.SkyrimSE.Skyrim.Furniture.PickaxeMiningTableMarkerNonPlayer
    ];

    public RecordAnalyzerResult? AnalyzeRecord(ContextualRecordAnalyzerParams<IPlacedObjectGetter> param)
    {
        var placedObject = param.Record;

        foreach (var linkedRef in placedObject.LinkedReferences)
        {
            if (!param.LinkCache.TryResolve<IPlacedObjectGetter>(linkedRef.Reference.FormKey, out var linkedObject)) continue;

            if (MiningFurniture.Contains(linkedObject.Base)) return null;
        }

        return new RecordAnalyzerResult(
            RecordTopic.Create(
                placedObject,
                NoFurnitureLinked.Format(),
                x => x.LinkedReferences));
    }
}
