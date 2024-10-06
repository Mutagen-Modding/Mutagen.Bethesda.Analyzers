using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Noggog;

namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Scene;

public class ConflictingVoiceTypesAnalyzer : IContextualRecordAnalyzer<ISceneGetter>
{
    public static readonly TopicDefinition<ISceneGetter, int, List<INpcGetter>, IFormLinkNullableGetter<IVoiceTypeGetter>> NpcsWithSameVoiceType = MutagenTopicBuilder.DevelopmentTopic(
            "NPCs with the same voice type in same scene",
            Severity.Suggestion)
        .WithFormatting<ISceneGetter, int, List<INpcGetter>, IFormLinkNullableGetter<IVoiceTypeGetter>>("Scene {0} includes {1} npcs {2} with the same voice type {3}");

    public IEnumerable<TopicDefinition> Topics { get; } = [NpcsWithSameVoiceType];

    public void AnalyzeRecord(ContextualRecordAnalyzerParams<ISceneGetter> param)
    {
        var scene = param.Record;

        if (!param.LinkCache.TryResolve<IQuestGetter>(scene.Quest.FormKey, out var quest)) return;

        var npcVoiceTypes = scene.Actors
            .Select(a => quest.Aliases.FirstOrDefault(x => x.ID == a.ID))
            .NotNull()
            .Select(a => a.UniqueActor.TryResolve(param.LinkCache))
            .NotNull()
            .GroupBy(x => x.Voice);

        foreach (var npcVoiceType in npcVoiceTypes)
        {
            var npcWithSameVoiceType = npcVoiceType.ToList();
            if (npcWithSameVoiceType.Count <= 1) continue;

            param.AddTopic(
                NpcsWithSameVoiceType.Format(scene, npcWithSameVoiceType.Count, npcWithSameVoiceType, npcVoiceType.Key));
        }
    }

    public IEnumerable<Func<ISceneGetter, object?>> FieldsOfInterest()
    {
        yield return x => x.Quest;
        yield return x => x.Actors;
    }
}
