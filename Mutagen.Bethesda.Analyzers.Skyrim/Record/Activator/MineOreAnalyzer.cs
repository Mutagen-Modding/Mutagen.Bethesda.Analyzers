using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Activator;

public class MineOreAnalyzer : IContextualRecordAnalyzer<IActivatorGetter>
{
    public static readonly TopicDefinition NoMineOreScript = MutagenTopicBuilder.DevelopmentTopic(
            "No MineOreScript",
            Severity.Warning)
        .WithoutFormatting("Mine Ore does not have a MineOreScript attached");

    public static readonly TopicDefinition NoOreProperty = MutagenTopicBuilder.DevelopmentTopic(
            "No Ore Property",
            Severity.Warning)
        .WithoutFormatting("Mine ore has no Ore property on MineOreScript");

    public static readonly TopicDefinition<IMiscItemGetter> IncorrectVeinOre = MutagenTopicBuilder.DevelopmentTopic(
            "Correct Vein/Ore",
            Severity.Warning)
        .WithFormatting<IMiscItemGetter>("Mine ore uses incorrect ore: {0}");

    public IEnumerable<TopicDefinition> Topics { get; } = [NoMineOreScript, IncorrectVeinOre];

    public RecordAnalyzerResult? AnalyzeRecord(ContextualRecordAnalyzerParams<IActivatorGetter> param)
    {
        var activator = param.Record;
        if (activator.EditorID is null) return null;
        if (!activator.EditorID.Contains("MineOre") && !activator.EditorID.Contains("MineGem")) return null;

        // No MineOreScript
        var script = activator.VirtualMachineAdapter?.Scripts.FirstOrDefault(s => string.Equals(s.Name, "MineOreScript", StringComparison.OrdinalIgnoreCase));
        if (script is null)
        {
            return new RecordAnalyzerResult(
                RecordTopic.Create(
                    activator,
                    NoMineOreScript.Format(),
                    x => x.VirtualMachineAdapter));
        }

        // Incorrect Vein/Ore
        var oreProperty = script.GetProperty<IScriptObjectPropertyGetter>("Ore");
        if (oreProperty is null)
        {
            return new RecordAnalyzerResult(
                RecordTopic.Create(
                    activator,
                    NoOreProperty.Format(),
                    x => x.VirtualMachineAdapter));
        }

        var oreLinks = oreProperty.EnumerateFormLinks().ToList();
        if (oreLinks.Count == 0)
        {
            return new RecordAnalyzerResult(
                RecordTopic.Create(
                    activator,
                    NoOreProperty.Format(),
                    x => x.VirtualMachineAdapter));
        }

        var ore = oreLinks[0].TryResolve<IMiscItemGetter>(param.LinkCache);
        if (ore is null)
        {
            return new RecordAnalyzerResult(
                RecordTopic.Create(
                    activator,
                    NoOreProperty.Format(),
                    x => x.VirtualMachineAdapter));
        }

        var oreSubStrings = ore.Name?.String?.Split(' ');
        if (oreSubStrings is null)
        {
            return new RecordAnalyzerResult(
                RecordTopic.Create(
                    activator,
                    NoOreProperty.Format(),
                    x => x.VirtualMachineAdapter));
        }

        foreach (var subString in oreSubStrings)
        {
            if (subString.Equals("Ore", StringComparison.Ordinal) || subString.Equals("Gem", StringComparison.OrdinalIgnoreCase)) continue;

            // When part of the ore name is found in the activator's editor ID, we assume it's the correct ore
            if (activator.EditorID.Contains(subString, StringComparison.OrdinalIgnoreCase)) return null;
        }

        return new RecordAnalyzerResult(
            RecordTopic.Create(
                activator,
                IncorrectVeinOre.Format(ore),
                x => x.VirtualMachineAdapter));
    }
}
