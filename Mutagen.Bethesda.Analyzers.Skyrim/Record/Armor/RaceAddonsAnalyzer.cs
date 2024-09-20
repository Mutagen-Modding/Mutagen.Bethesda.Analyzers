using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Noggog;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Armor;

public class RaceAddonsAnalyzer : IContextualRecordAnalyzer<IArmorGetter>
{
    public static readonly TopicDefinition<string?> ArmorMissingRaceAddons = MutagenTopicBuilder.DevelopmentTopic(
            "Armor is missing race addons",
            Severity.Warning)
        .WithFormatting<string?>("Missing race addon for race: {0}");

    public IEnumerable<TopicDefinition> Topics => [ArmorMissingRaceAddons];

    private static readonly HashSet<IFormLinkGetter<IRaceGetter>> DefaultPlayerRaces =
    [
        FormKeys.SkyrimSE.Skyrim.Race.ArgonianRace, FormKeys.SkyrimSE.Skyrim.Race.ArgonianRaceVampire,
        FormKeys.SkyrimSE.Skyrim.Race.BretonRace, FormKeys.SkyrimSE.Skyrim.Race.BretonRaceVampire,
        FormKeys.SkyrimSE.Skyrim.Race.DarkElfRace, FormKeys.SkyrimSE.Skyrim.Race.DarkElfRaceVampire,
        FormKeys.SkyrimSE.Skyrim.Race.HighElfRace, FormKeys.SkyrimSE.Skyrim.Race.HighElfRaceVampire,
        FormKeys.SkyrimSE.Skyrim.Race.ImperialRace, FormKeys.SkyrimSE.Skyrim.Race.ImperialRaceVampire,
        FormKeys.SkyrimSE.Skyrim.Race.KhajiitRace, FormKeys.SkyrimSE.Skyrim.Race.KhajiitRaceVampire,
        FormKeys.SkyrimSE.Skyrim.Race.NordRace, FormKeys.SkyrimSE.Skyrim.Race.NordRaceVampire,
        FormKeys.SkyrimSE.Skyrim.Race.OrcRace, FormKeys.SkyrimSE.Skyrim.Race.OrcRaceVampire,
        FormKeys.SkyrimSE.Skyrim.Race.RedguardRace, FormKeys.SkyrimSE.Skyrim.Race.RedguardRaceVampire,
        FormKeys.SkyrimSE.Skyrim.Race.WoodElfRace, FormKeys.SkyrimSE.Skyrim.Race.WoodElfRaceVampire
    ];

    public RecordAnalyzerResult? AnalyzeRecord(ContextualRecordAnalyzerParams<IArmorGetter> param)
    {
        var armor = param.Record;

        // Armor with template armor inherit all relevant data from the template armor and should not be checked themselves
        if (!armor.TemplateArmor.IsNull) return null;

        // Non-playable cannot be equipped by the player and is usually required to be universally compatible
        if (armor.MajorFlags.HasFlag(Bethesda.Skyrim.Armor.MajorFlag.NonPlayable)) return null;

        // Exclude non player race armor
        if (armor.Race.FormKey != FormKeys.SkyrimSE.Skyrim.Race.DefaultRace.FormKey
            && !DefaultPlayerRaces.Contains(armor.Race)) return null;

        // Build list of all races in armor addons
        var missingRaces = new List<IFormLinkGetter<IRaceGetter>>(DefaultPlayerRaces);
        foreach (var armorAddon in armor.Armature)
        {
            var addon = armorAddon.TryResolve(param.LinkCache);
            if (addon == null) continue;

            foreach (var additionalRace in addon.AdditionalRaces)
            {
                missingRaces.Remove(additionalRace.FormKey);
            }
            missingRaces.Remove(addon.Race.FormKey);
        }

        var result = new RecordAnalyzerResult();

        foreach (var race in missingRaces)
        {
            if (param.LinkCache.TryResolve(race, out var r))
            {
                result.AddTopic(RecordTopic.Create(
                    armor,
                    ArmorMissingRaceAddons.Format(r.EditorID),
                    x => x.EditorID));
            }
        }

        return result;
    }
}
