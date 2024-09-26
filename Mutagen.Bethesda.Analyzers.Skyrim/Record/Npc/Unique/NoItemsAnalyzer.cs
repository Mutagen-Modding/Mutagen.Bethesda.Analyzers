using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Npc.Unique;

public class NoItemsAnalyzer : IContextualRecordAnalyzer<INpcGetter>
{
    public static readonly TopicDefinition NoItems = MutagenTopicBuilder.DevelopmentTopic(
            "Empty Inventory",
            Severity.Warning)
        .WithoutFormatting("Npc has no items in inventory");

    public IEnumerable<TopicDefinition> Topics { get; } = [NoItems];

    public RecordAnalyzerResult? AnalyzeRecord(ContextualRecordAnalyzerParams<INpcGetter> param)
    {
        var npc = param.Record;
        if (!npc.IsEligibleForTest()) return null;

        // Skip NPCs using templates for inventory
        if (npc.Configuration.TemplateFlags.HasFlag(NpcConfiguration.TemplateFlag.Inventory)) return null;

        if (npc.Items is null || npc.Items.Count == 0)
        {
            return new RecordAnalyzerResult(
                RecordTopic.Create(
                    npc,
                    NoItems.Format(),
                    x => x.Configuration));
        }

        return null;
    }
}
