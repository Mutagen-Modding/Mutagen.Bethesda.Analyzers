using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Contextual;

public class UnownedBedAnalyzer : IContextualAnalyzer
{
    public static readonly TopicDefinition<string, string?> UnownedBed = MutagenTopicBuilder.FromDiscussion(
            0,
            "Unowned Bed in Owned Cell",
            Severity.Suggestion)
        .WithFormatting<string, string?>("Unowned bed placement {0} in owned cell {1}");

    public IEnumerable<TopicDefinition> Topics { get; } = [UnownedBed];

    public ContextualAnalyzerResult Analyze(ContextualAnalyzerParams param)
    {
        var result = new ContextualAnalyzerResult();

        foreach (var cell in param.LinkCache.PriorityOrder.WinningOverrides<ICellGetter>())
        {
            if (cell.IsExteriorCell()) continue;

            // If the cell is public or unowned, the bed can be unowned too
            if (cell.IsPublic() || cell.Owner.IsNull) continue;

            foreach (var placedObject in cell.GetAllPlaced(param.LinkCache).OfType<IPlacedObjectGetter>())
            {
                // Owned beds are not a problem
                if (!placedObject.Owner.IsNull) continue;

                if (!param.LinkCache.TryResolve<IFurnitureGetter>(placedObject.Base.FormKey, out var furniture)) continue;
                if (!furniture.IsBed()) continue;

                result.AddTopic(
                    ContextualTopic.Create(
                        placedObject,
                        UnownedBed.Format(placedObject.FormKey.ToString(), cell.EditorID)
                    )
                );
            }
        }

        return result;
    }
}
