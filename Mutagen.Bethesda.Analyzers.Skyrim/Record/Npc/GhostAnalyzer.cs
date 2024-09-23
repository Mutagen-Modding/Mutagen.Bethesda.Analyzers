using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Npc;

public class GhostKeywordAnalyzer : IContextualRecordAnalyzer<INpcGetter>
{
    public static readonly TopicDefinition GhostScriptMissingKeyword = MutagenTopicBuilder.DevelopmentTopic(
            "Ghost With Script Missing Keyword",
            Severity.Suggestion)
        .WithoutFormatting("Npc has ghost script but no ghost keyword");

    public static readonly TopicDefinition GhostFlagMissingKeyword = MutagenTopicBuilder.DevelopmentTopic(
            "Ghost With Flag Missing Keyword",
            Severity.Suggestion)
        .WithoutFormatting("Npc has ghost flag but no ghost keyword");

    public IEnumerable<TopicDefinition> Topics { get; } = [GhostScriptMissingKeyword, GhostFlagMissingKeyword];

    public RecordAnalyzerResult AnalyzeRecord(ContextualRecordAnalyzerParams<INpcGetter> param)
    {
        var npc = param.Record;

        var hasKeyword = npc.HasKeyword(FormKeys.SkyrimSE.Skyrim.Keyword.ActorTypeGhost);
        var hasFlag = npc.Configuration.Flags.HasFlag(NpcConfiguration.Flag.IsGhost);
        var hasScript = npc.HasScript("defaultGhostScript");

        var results = new RecordAnalyzerResult();

        if (hasScript && !hasKeyword)
        {
            results.AddTopic(
                RecordTopic.Create(
                    npc,
                    GhostScriptMissingKeyword.Format(),
                    x => x.VirtualMachineAdapter));
        }

        if (hasFlag && !hasKeyword)
        {
            results.AddTopic(
                RecordTopic.Create(
                    npc,
                    GhostFlagMissingKeyword.Format(),
                    x => x.VirtualMachineAdapter));
        }

        return results;
    }
}
