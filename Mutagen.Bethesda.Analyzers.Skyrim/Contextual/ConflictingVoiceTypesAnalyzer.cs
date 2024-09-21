using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Contextual;

public class ConflictingVoiceTypesAnalyzer : IContextualAnalyzer
{
    public static readonly TopicDefinition<ICellGetter, int, List<IFormLinkGetter<INpcGetter>>, IFormLinkGetter<IVoiceTypeGetter>> NpcsWithSameVoiceType = MutagenTopicBuilder.DevelopmentTopic(
            "NPCs with the same voice type in same cell",
            Severity.Suggestion)
        .WithFormatting<ICellGetter, int, List<IFormLinkGetter<INpcGetter>>, IFormLinkGetter<IVoiceTypeGetter>>("Cell {0} includes {1} npcs {2} with the same voice type {3}");

    public IEnumerable<TopicDefinition> Topics { get; } = [NpcsWithSameVoiceType];

    public ContextualAnalyzerResult Analyze(ContextualAnalyzerParams param)
    {
        var result = new ContextualAnalyzerResult();

        foreach (var cell in param.LinkCache.PriorityOrder.WinningOverrides<ICellGetter>())
        {
            if (cell.IsExteriorCell()) continue;

            var npcVoiceTypes = new Dictionary<IFormLinkGetter<INpcGetter>, IFormLinkGetter<IVoiceTypeGetter>>();
            foreach (var placedNpc in cell.GetAllPlaced(param.LinkCache).OfType<IPlacedNpcGetter>())
            {
                if (!param.LinkCache.TryResolve<INpcGetter>(placedNpc.Base.FormKey, out var npc)) continue;
                if (!npc.IsUnique()) continue;
                if (npc.Voice.IsNull) continue;

                npcVoiceTypes.TryAdd(npc.ToLink(), npc.Voice);
            }

            foreach (var grouping in npcVoiceTypes.GroupBy(x => x.Value))
            {
                var npcs = grouping.Select(x => x.Key).ToList();
                var count = npcs.Count;
                if (count <= 1) continue;

                result.AddTopic(
                    ContextualTopic.Create(
                        cell,
                        NpcsWithSameVoiceType.Format(cell, count, npcs, grouping.Key)
                    )
                );
            }
        }

        return result;
    }
}
