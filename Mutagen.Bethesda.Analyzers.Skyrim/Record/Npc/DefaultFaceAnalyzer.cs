using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
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

    public RecordAnalyzerResult AnalyzeRecord(ContextualRecordAnalyzerParams<INpcGetter> param)
    {
        var npc = param.Record;
        var result = new RecordAnalyzerResult();

        if (!npc.HasKeyword(FormKeys.SkyrimSE.Skyrim.Keyword.ActorTypeNPC)) return result;

        var race = npc.Race.TryResolve(param.LinkCache);
        if (race is null || !race.Flags.HasFlag(Race.Flag.FaceGenHead)) return result;

        if (npc.FaceMorph is null)
        {
            result.AddTopic(
                RecordTopic.Create(
                    npc,
                    DefaultFaceMorph.Format(),
                    x => x.FaceMorph));
        }

        if (npc.FaceParts is null)
        {
            result.AddTopic(
                RecordTopic.Create(
                    npc,
                    DefaultFaceParts.Format(),
                    x => x.FaceParts));
        }

        return result;
    }
}
