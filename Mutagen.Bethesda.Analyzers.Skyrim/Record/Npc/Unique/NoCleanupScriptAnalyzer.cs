using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Npc.Unique;

public class NoCleanupScriptAnalyzer : IContextualRecordAnalyzer<INpcGetter>
{
    private const string CleanupScriptName = "WIDeadBodyCleanupScript";

    public static readonly TopicDefinition NoCleanupScript = MutagenTopicBuilder.DevelopmentTopic(
            "No Cleanup Script",
            Severity.Warning)
        .WithoutFormatting("Npc has no cleanup script");

    public static readonly TopicDefinition DeathContainerPropertyNotFilled = MutagenTopicBuilder.DevelopmentTopic(
            "Death Container Not Found",
            Severity.Warning)
        .WithoutFormatting("Death container property is not filled in cleanup script");

    public static readonly TopicDefinition WIPropertyNotFilled = MutagenTopicBuilder.DevelopmentTopic(
            "WI quest Property Not Found",
            Severity.Warning)
        .WithoutFormatting("WI quest property is not filled in cleanup script");

    public IEnumerable<TopicDefinition> Topics { get; } = [NoCleanupScript, DeathContainerPropertyNotFilled, WIPropertyNotFilled];

    public void AnalyzeRecord(ContextualRecordAnalyzerParams<INpcGetter> param)
    {
        var npc = param.Record;
        if (!npc.IsEligibleForTest()) return;

        // Skip NPCs using templates for scripts
        if (npc.Configuration.TemplateFlags.HasFlag(NpcConfiguration.TemplateFlag.Script)) return;

        var script = npc.GetScript(CleanupScriptName);
        if (script is null)
        {
            param.AddTopic(
                NoCleanupScript.Format(),
                x => x.VirtualMachineAdapter);
            return;
        }

        var deathContainer = script.GetProperty<IScriptObjectPropertyGetter>("DeathContainer");
        if (deathContainer is null)
        {
            param.AddTopic(
                DeathContainerPropertyNotFilled.Format(),
                x => x.VirtualMachineAdapter);
        }

        var wiQuest = script.GetProperty<IScriptObjectPropertyGetter>("WI");
        if (wiQuest is null)
        {
            param.AddTopic(
                WIPropertyNotFilled.Format(),
                x => x.VirtualMachineAdapter);
        }
    }
}
