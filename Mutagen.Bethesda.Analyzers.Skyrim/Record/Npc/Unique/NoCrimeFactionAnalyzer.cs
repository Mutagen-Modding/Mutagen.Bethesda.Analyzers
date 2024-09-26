using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Npc.Unique;

public class NoCrimeFactionAnalyzer : IContextualRecordAnalyzer<INpcGetter>
{
    public static readonly TopicDefinition NoCrimeFaction = MutagenTopicBuilder.DevelopmentTopic(
            "No Crime Faction",
            Severity.Warning)
        .WithoutFormatting("Npc has no crime faction");

    public IEnumerable<TopicDefinition> Topics { get; } = [NoCrimeFaction];

    public RecordAnalyzerResult? AnalyzeRecord(ContextualRecordAnalyzerParams<INpcGetter> param)
    {
        var npc = param.Record;
        if (!npc.IsEligibleForTest()) return null;

        // Skip NPCs using templates for factions
        if (npc.Configuration.TemplateFlags.HasFlag(NpcConfiguration.TemplateFlag.Factions)) return null;

        // Skip NPCs who don't care about crime
        if (npc.AIData.Responsibility == Responsibility.NoCrime) return null;

        if (npc.CrimeFaction.IsNull)
        {
            return new RecordAnalyzerResult(
                RecordTopic.Create(
                    npc,
                    NoCrimeFaction.Format(),
                    x => x.Configuration));
        }

        return null;
    }
}
