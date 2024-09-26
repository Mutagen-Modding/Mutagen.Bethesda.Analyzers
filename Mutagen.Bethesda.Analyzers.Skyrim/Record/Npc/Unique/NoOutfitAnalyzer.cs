using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Npc.Unique;

public class NoOutfitAnalyzer : IContextualRecordAnalyzer<INpcGetter>
{
    public static readonly TopicDefinition NoOutfit = MutagenTopicBuilder.DevelopmentTopic(
            "No Outfit",
            Severity.Warning)
        .WithoutFormatting("Npc doesn't wear any outfit");

    public IEnumerable<TopicDefinition> Topics { get; } = [NoOutfit];

    public RecordAnalyzerResult? AnalyzeRecord(ContextualRecordAnalyzerParams<INpcGetter> param)
    {
        var npc = param.Record;
        if (!npc.IsEligibleForTest()) return null;

        // Skip NPCs using templates for inventory
        if (npc.Configuration.TemplateFlags.HasFlag(NpcConfiguration.TemplateFlag.Inventory)) return null;

        if (npc.DefaultOutfit.IsNull)
        {
            return new RecordAnalyzerResult(
                RecordTopic.Create(
                    npc,
                    NoOutfit.Format(),
                    x => x.Configuration));
        }

        return null;
    }
}
