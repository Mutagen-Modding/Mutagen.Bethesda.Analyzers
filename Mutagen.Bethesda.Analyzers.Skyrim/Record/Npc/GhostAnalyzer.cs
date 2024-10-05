using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
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

    public void AnalyzeRecord(ContextualRecordAnalyzerParams<INpcGetter> param)
    {
        var npc = param.Record;

        var hasKeyword = npc.HasKeyword(FormKeys.SkyrimSE.Skyrim.Keyword.ActorTypeGhost);
        var hasFlag = npc.Configuration.Flags.HasFlag(NpcConfiguration.Flag.IsGhost);
        var hasScript = npc.HasScript("defaultGhostScript");

        if (hasScript && !hasKeyword)
        {
            param.AddTopic(
                GhostScriptMissingKeyword.Format(),
                x => x.VirtualMachineAdapter);
        }

        if (hasFlag && !hasKeyword)
        {
            param.AddTopic(
                GhostFlagMissingKeyword.Format(),
                x => x.VirtualMachineAdapter);
        }
    }
}
