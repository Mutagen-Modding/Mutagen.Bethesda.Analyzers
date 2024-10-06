using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.Analyzers.Skyrim.Contextual;

public class NorthMarkerAnalyzer : IContextualAnalyzer
{
    public static readonly TopicDefinition NoNorthMarker = MutagenTopicBuilder.DevelopmentTopic(
            "No North Marker",
            Severity.Suggestion)
        .WithoutFormatting("Missing north marker");

    public static readonly TopicDefinition<IEnumerable<IPlacedObjectGetter>> MoreThanOneNorthMarker = MutagenTopicBuilder.DevelopmentTopic(
            "More Than One North Marker",
            Severity.Suggestion)
        .WithFormatting<IEnumerable<IPlacedObjectGetter>>("Cell has multiple north markers {0} when only one is permitted");

    public IEnumerable<TopicDefinition> Topics { get; } = [NoNorthMarker, MoreThanOneNorthMarker];

    public void Analyze(ContextualAnalyzerParams param)
    {
        foreach (var cell in param.LinkCache.PriorityOrder.WinningOverrides<ICellGetter>())
        {
            if (cell.IsExteriorCell()) continue;

            var northMarkers = cell.GetAllPlaced(param.LinkCache)
                .OfType<IPlacedObjectGetter>()
                .Where(placed => placed.Base.FormKey == FormKeys.SkyrimSE.Skyrim.Static.NorthMarker.FormKey)
                .ToArray();

            if (northMarkers.Length == 0)
            {
                param.AddTopic(NoNorthMarker.Format());
            }

            if (northMarkers.Length > 1)
            {
                param.AddTopic(MoreThanOneNorthMarker.Format(northMarkers));
            }
        }
    }
}
