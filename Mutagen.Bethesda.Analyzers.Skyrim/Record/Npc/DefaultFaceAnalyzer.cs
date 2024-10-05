using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Npc;

public class DefaultFaceAnalyzer : IContextualRecordAnalyzer<INpcGetter>
{
    public static readonly TopicDefinition DefaultFaceMorph = MutagenTopicBuilder.DevelopmentTopic(
            "Default Face Morph",
            Severity.Suggestion)
        .WithoutFormatting("Npc has no custom face morph data");

    public static readonly TopicDefinition DefaultFaceParts = MutagenTopicBuilder.DevelopmentTopic(
            "Default Face Parts",
            Severity.Suggestion)
        .WithoutFormatting("Npc has no custom face parts data");

    public IEnumerable<TopicDefinition> Topics { get; } = [DefaultFaceMorph, DefaultFaceParts];

    public void AnalyzeRecord(ContextualRecordAnalyzerParams<INpcGetter> param)
    {
        var npc = param.Record;

        if (!npc.HasKeyword(FormKeys.SkyrimSE.Skyrim.Keyword.ActorTypeNPC)) return;

        var race = npc.Race.TryResolve(param.LinkCache);
        if (race is null || !race.Flags.HasFlag(Race.Flag.FaceGenHead)) return;

        if (npc.FaceMorph is null)
        {
            param.AddTopic(
                DefaultFaceMorph.Format(),
                x => x.FaceMorph);
        }

        if (npc.FaceParts is null)
        {
            param.AddTopic(
                DefaultFaceParts.Format(),
                x => x.FaceParts);
        }
    }
}
