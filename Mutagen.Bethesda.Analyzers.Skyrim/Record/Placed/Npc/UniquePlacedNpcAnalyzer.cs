using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Placed.Npc;

public class UniquePlacedNpcAnalyzer : IContextualRecordAnalyzer<IPlacedNpcGetter>
{
    public static readonly TopicDefinition UniqueNpcNotInPersistenceLocation = MutagenTopicBuilder.DevelopmentTopic(
            "Unique NPC not in Persistence Location",
            Severity.Error)
        .WithoutFormatting("Placed NPCs should be placed in persistence location, otherwise they might not be loaded");

    public static readonly TopicDefinition UniqueNpcWithoutPersistenceLocation = MutagenTopicBuilder.DevelopmentTopic(
            "Unique NPC without Persistence Location",
            Severity.Error)
        .WithoutFormatting("Placed NPCs should have a persistence location if the NPC is unique, excludes always persistent npc or initially disabled NPCs");

    public IEnumerable<TopicDefinition> Topics { get; } = [];

    public RecordAnalyzerResult? AnalyzeRecord(ContextualRecordAnalyzerParams<IPlacedNpcGetter> param)
    {
        var placedNpc = param.Record;
        var result = new RecordAnalyzerResult();

        if (placedNpc.MajorFlags.HasFlag(PlacedNpc.MajorFlag.InitiallyDisabled)) return result;
        if (placedNpc.MajorFlags.HasFlag(PlacedNpc.MajorFlag.StartsDead)) return result;

        if (!placedNpc.Base.TryResolveSimpleContext(param.LinkCache, out var context)) return result;

        var npc = context.Record;
        if (!npc.IsUnique()) return result;

        if (context.Parent?.Record is ICellGetter cell)
        {
            var persistLocation = placedNpc.PersistentLocation.TryResolve(param.LinkCache);
            if (persistLocation is not null
                && cell.GetAllLocations(param.LinkCache)
                    .Select(location => location.FormKey)
                    .Contains(persistLocation.FormKey))
            {
                result.AddTopic(
                    RecordTopic.Create(
                        placedNpc,
                        UniqueNpcNotInPersistenceLocation.Format(),
                        x => x.PersistentLocation
                    )
                );
            }
        }

        if (placedNpc.PersistentLocation.IsNull)
        {
            result.AddTopic(
                RecordTopic.Create(
                    placedNpc,
                    UniqueNpcWithoutPersistenceLocation.Format(),
                    x => x.PersistentLocation
                )
            );
        }

        return result;
    }
}
