﻿using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Noggog;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Armor;

public class FootstepAnalyzer : IContextualRecordAnalyzer<IArmorGetter>
{
    public static readonly TopicDefinition<string, string> ArmorMatchingFootstepArmorType = MutagenTopicBuilder.FromDiscussion(
            0,
            "Footsteps on armor don't match their equipped armor type",
            Severity.Suggestion)
        .WithFormatting<string, string>("Armor has armor type {0} but armor addon doesn't have footstep {1}");

    public static readonly TopicDefinition ArmorMissingFootstep = MutagenTopicBuilder.FromDiscussion(
            0,
            "Armor has no footstep sound",
            Severity.Warning)
        .WithoutFormatting("Armor has no armor addon that adds footstep sounds");

    public static readonly TopicDefinition<string, string> ArmorDuplicateFootstep = MutagenTopicBuilder.FromDiscussion(
            0,
            "Armor has more than one armor addon that adds footstep sound",
            Severity.Suggestion)
        .WithFormatting<string, string>("Armor has multiple armor addons {0} that have footstep sounds which are enabled for the same races {1}");

    public IEnumerable<TopicDefinition> Topics => [ArmorMatchingFootstepArmorType, ArmorMissingFootstep, ArmorDuplicateFootstep];

    public RecordAnalyzerResult? AnalyzeRecord(ContextualRecordAnalyzerParams<IArmorGetter> param)
    {
        var result = new RecordAnalyzerResult();
        var armor = param.Record;

        // Armor with template armor inherit all relevant data from the template armor and should not be checked themselves
        if (!armor.TemplateArmor.IsNull) return null;

        // Only armor with feet slots are relevant for footsteps
        if (armor.BodyTemplate is null || !armor.BodyTemplate.FirstPersonFlags.HasFlag(BipedObjectFlag.Feet)) return null;

        var armorAddons = armor.Armature
            .Select(armorAddonLink => armorAddonLink.TryResolve(param.LinkCache))
            .NotNull()
            .ToList();

        // Check duplicate footsteps
        var armorAddonRaces = armorAddons
            .Where(x => !x.FootstepSound.IsNull)
            .Select(x => (x, (List<FormKey>) [x.Race.FormKey, ..x.AdditionalRaces.Select(r => r.FormKey)])).ToList();

        foreach (var (armorAddon, races) in armorAddonRaces)
        {
            foreach (var (otherArmorAddon, otherRaces) in armorAddonRaces)
            {
                if (armorAddon.FormKey != otherArmorAddon.FormKey)
                {
                    var duplicateRaces = races.Intersect(otherRaces).ToArray();
                    if (duplicateRaces.Any())
                    {
                        result.AddTopic(
                            RecordTopic.Create(
                                armor,
                                ArmorDuplicateFootstep.Format(armorAddon.EditorID + ", " + otherArmorAddon.EditorID, string.Join(", ", duplicateRaces.Select(r => r.ToString()))),
                                x => x.Armature));
                    }
                }
            }
        }

        // Check if the footstep sound is correct
        var (correctFootstepSound, correctFootstepEditorID) = armor.BodyTemplate.ArmorType switch
        {
            ArmorType.LightArmor => (FormKeys.SkyrimSE.Skyrim.FootstepSet.FSTArmorLightFootstepSet.FormKey, nameof(FormKeys.SkyrimSE.Skyrim.FootstepSet.FSTArmorLightFootstepSet)),
            ArmorType.HeavyArmor => (FormKeys.SkyrimSE.Skyrim.FootstepSet.FSTArmorHeavyFootstepSet.FormKey, nameof(FormKeys.SkyrimSE.Skyrim.FootstepSet.FSTArmorHeavyFootstepSet)),
            ArmorType.Clothing => (FormKeys.SkyrimSE.Skyrim.FootstepSet.DefaultFootstepSet.FormKey, nameof(FormKeys.SkyrimSE.Skyrim.FootstepSet.DefaultFootstepSet)),
            _ => throw new InvalidOperationException()
        };

        foreach (var armorAddon in armorAddons)
        {
            if (armorAddon.FootstepSound.FormKey != correctFootstepSound)
            {
                result.AddTopic(
                    RecordTopic.Create(
                        armorAddon,
                        ArmorMatchingFootstepArmorType.Format(armor.BodyTemplate.ArmorType.ToString(), correctFootstepEditorID),
                        x => x.FootstepSound));
            }
        }

        // Check if there are any footstep sounds
        if (armorAddons.Count == 0 || armorAddons.TrueForAll(x => x.FootstepSound.IsNull))
        {
            result.AddTopic(
                RecordTopic.Create(
                    armor,
                    ArmorMissingFootstep.Format(),
                    x => x.Armature));
        }

        return result;
    }
}