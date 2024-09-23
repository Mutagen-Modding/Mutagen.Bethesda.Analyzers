using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Npc;

public class MerchantAnalyzer : IContextualRecordAnalyzer<INpcGetter>
{
    public static readonly TopicDefinition MerchantWithoutSpecialization = MutagenTopicBuilder.DevelopmentTopic(
            "Merchant without specialization",
            Severity.Warning)
        .WithoutFormatting("Npc has JobMerchant faction, but doesn't have any other Job faction");

    public IEnumerable<TopicDefinition> Topics { get; } = [MerchantWithoutSpecialization];

    public RecordAnalyzerResult? AnalyzeRecord(ContextualRecordAnalyzerParams<INpcGetter> param)
    {
        var npc = param.Record;
        var isMerchant = npc.HasFaction(param.LinkCache, editorId =>
            editorId is not null && editorId.Contains("JobMerchant", StringComparison.OrdinalIgnoreCase));
        if (!isMerchant) return null;

        var hasSpecialization = npc.HasFaction(param.LinkCache, editorId =>
            editorId is not null
            && !editorId.Contains("JobMerchant", StringComparison.OrdinalIgnoreCase)
            && !editorId.Contains("JobTrainer", StringComparison.OrdinalIgnoreCase)
            && editorId.Contains("Job", StringComparison.OrdinalIgnoreCase));

        if (hasSpecialization) return null;

        return new RecordAnalyzerResult(
            RecordTopic.Create(
                npc,
                MerchantWithoutSpecialization.Format(),
                x => x.Factions));
    }
}
