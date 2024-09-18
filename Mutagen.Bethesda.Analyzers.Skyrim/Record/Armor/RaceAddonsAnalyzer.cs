using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Noggog;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Armor;

public class RaceAddonsAnalyzer : IContextualRecordAnalyzer<IArmorGetter>
{
    public static readonly TopicDefinition<string?> ArmorMissingRaceAddons = MutagenTopicBuilder.FromDiscussion(
            0,
            "Armor is missing race addons",
            Severity.Warning)
        .WithFormatting<string?>("Missing race addon for race: {0}");

    public IEnumerable<TopicDefinition> Topics => [ArmorMissingRaceAddons];

    private static readonly List<FormKey> DefaultPlayerRaces =
    [
        FormKeys.SkyrimSE.Skyrim.Race.ArgonianRace.FormKey, FormKeys.SkyrimSE.Skyrim.Race.ArgonianRaceVampire.FormKey,
        FormKeys.SkyrimSE.Skyrim.Race.BretonRace.FormKey, FormKeys.SkyrimSE.Skyrim.Race.BretonRaceVampire.FormKey,
        FormKeys.SkyrimSE.Skyrim.Race.DarkElfRace.FormKey, FormKeys.SkyrimSE.Skyrim.Race.DarkElfRaceVampire.FormKey,
        FormKeys.SkyrimSE.Skyrim.Race.HighElfRace.FormKey, FormKeys.SkyrimSE.Skyrim.Race.HighElfRaceVampire.FormKey,
        FormKeys.SkyrimSE.Skyrim.Race.ImperialRace.FormKey, FormKeys.SkyrimSE.Skyrim.Race.ImperialRaceVampire.FormKey,
        FormKeys.SkyrimSE.Skyrim.Race.KhajiitRace.FormKey, FormKeys.SkyrimSE.Skyrim.Race.KhajiitRaceVampire.FormKey,
        FormKeys.SkyrimSE.Skyrim.Race.NordRace.FormKey, FormKeys.SkyrimSE.Skyrim.Race.NordRaceVampire.FormKey,
        FormKeys.SkyrimSE.Skyrim.Race.OrcRace.FormKey, FormKeys.SkyrimSE.Skyrim.Race.OrcRaceVampire.FormKey,
        FormKeys.SkyrimSE.Skyrim.Race.RedguardRace.FormKey, FormKeys.SkyrimSE.Skyrim.Race.RedguardRaceVampire.FormKey,
        FormKeys.SkyrimSE.Skyrim.Race.WoodElfRace.FormKey, FormKeys.SkyrimSE.Skyrim.Race.WoodElfRaceVampire.FormKey
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
            && !DefaultPlayerRaces.Contains(armor.Race.FormKey)) return null;

        // Build list of all races in armor addons
        var missingRaces = new List<FormKey>(DefaultPlayerRaces);
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
            if (param.LinkCache.TryResolve<IRaceGetter>(race, out var r))
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
