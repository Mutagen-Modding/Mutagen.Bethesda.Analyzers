using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Contextual;

public class ConflictingVoiceTypesAnalyzer : IContextualAnalyzer
{

    public static readonly TopicDefinition<string?, int, string, string?> NpcsWithSameVoiceType = MutagenTopicBuilder.FromDiscussion(
            0,
            "NPCs with the same voice type in same cell",
            Severity.Suggestion)
        .WithFormatting<string?, int, string, string?>("Cell {0} includes {1} npcs {2} with the same voice type {3}");

    public IEnumerable<TopicDefinition> Topics { get; } = [NpcsWithSameVoiceType];

    public ContextualAnalyzerResult Analyze(ContextualAnalyzerParams param)
    {
        var result = new ContextualAnalyzerResult();

        foreach (var cell in param.LinkCache.PriorityOrder.WinningOverrides<ICellGetter>())
        {
            if (cell.IsExteriorCell()) continue;

            var npcVoiceTypes = new Dictionary<INpcGetter, FormKey>();
            foreach (var placedNpc in cell.GetAllPlaced(param.LinkCache).OfType<IPlacedNpcGetter>())
            {
                if (!param.LinkCache.TryResolve<INpcGetter>(placedNpc.Base.FormKey, out var npc)) continue;
                if (!npc.IsUnique()) continue;
                if (npc.Voice.IsNull) continue;

                npcVoiceTypes.TryAdd(npc, npc.Voice.FormKey);
            }

            foreach (var grouping in npcVoiceTypes.GroupBy(x => x.Value))
            {
                var npcNames = grouping.Select(x => x.Key.EditorID ?? x.Key.FormKey.ToString()).ToList();
                var count = npcNames.Count;
                if (count <= 1) continue;

                if (param.LinkCache.TryResolve<IVoiceTypeGetter>(grouping.Key, out var voiceType))
                {
                    result.AddTopic(
                        ContextualTopic.Create(
                            cell,
                            NpcsWithSameVoiceType.Format(cell.EditorID, count, string.Join(", ", npcNames), voiceType.EditorID)
                        )
                    );
                }
            }
        }


        return result;
    }
}
