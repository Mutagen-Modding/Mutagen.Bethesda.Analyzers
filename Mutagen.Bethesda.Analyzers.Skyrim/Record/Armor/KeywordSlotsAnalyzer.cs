using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Armor;

public class KeywordSlotsAnalyzer : IIsolatedRecordAnalyzer<IArmorGetter>
{
    public static readonly TopicDefinition<string, string> ArmorMatchingKeywordSlots = MutagenTopicBuilder.FromDiscussion(
            0,
            "Armor keywords don't match their equipped slot",
            Severity.Suggestion)
        .WithFormatting<string, string>(" Equipped in slot {0} but doesn't have keyword {1}");

    public IEnumerable<TopicDefinition> Topics => [ArmorMatchingKeywordSlots];

    public RecordAnalyzerResult? AnalyzeRecord(IsolatedRecordAnalyzerParams<IArmorGetter> param)
    {
        var armor = param.Record;

        // Armor with template armor inherit all relevant data from the template armor and should not be checked themselves
        if (!armor.TemplateArmor.IsNull) return null;

        // Armor with no slots are not relevant
        if (armor.BodyTemplate is null) return null;

        // Ignore armor with no keywords, these are usually skin armor
        if (armor.Keywords is null) return null;

        // Armor type dependent conditions for main armor slots
        List<(BipedObjectFlag Slots, FormLink<IKeywordGetter> Keyword, string KeywordEditorID)> conditions = armor.BodyTemplate.ArmorType switch
        {
            ArmorType.Clothing =>
            [
                (BipedObjectFlag.Body, FormKeys.SkyrimSE.Skyrim.Keyword.ClothingBody, nameof(FormKeys.SkyrimSE.Skyrim.Keyword.ClothingBody)),
                (BipedObjectFlag.Feet, FormKeys.SkyrimSE.Skyrim.Keyword.ClothingFeet, nameof(FormKeys.SkyrimSE.Skyrim.Keyword.ClothingFeet)),
                (BipedObjectFlag.Hands, FormKeys.SkyrimSE.Skyrim.Keyword.ClothingHands, nameof(FormKeys.SkyrimSE.Skyrim.Keyword.ClothingHands)),
                (BipedObjectFlag.Head | BipedObjectFlag.Hair, FormKeys.SkyrimSE.Skyrim.Keyword.ClothingHead, nameof(FormKeys.SkyrimSE.Skyrim.Keyword.ClothingHead)),
            ],
            _ =>
            [
                (BipedObjectFlag.Body, FormKeys.SkyrimSE.Skyrim.Keyword.ArmorCuirass, nameof(FormKeys.SkyrimSE.Skyrim.Keyword.ArmorCuirass)),
                (BipedObjectFlag.Feet, FormKeys.SkyrimSE.Skyrim.Keyword.ArmorBoots, nameof(FormKeys.SkyrimSE.Skyrim.Keyword.ArmorBoots)),
                (BipedObjectFlag.Hands, FormKeys.SkyrimSE.Skyrim.Keyword.ArmorGauntlets, nameof(FormKeys.SkyrimSE.Skyrim.Keyword.ArmorGauntlets)),
                (BipedObjectFlag.Head | BipedObjectFlag.Hair, FormKeys.SkyrimSE.Skyrim.Keyword.ArmorHelmet, nameof(FormKeys.SkyrimSE.Skyrim.Keyword.ArmorHelmet)),
            ]
        };

        // Armor type independent conditions
        conditions.AddRange([
            (BipedObjectFlag.Shield, FormKeys.SkyrimSE.Skyrim.Keyword.ArmorShield, nameof(FormKeys.SkyrimSE.Skyrim.Keyword.ArmorShield)),
            (BipedObjectFlag.Circlet, FormKeys.SkyrimSE.Skyrim.Keyword.ClothingCirclet, nameof(FormKeys.SkyrimSE.Skyrim.Keyword.ClothingCirclet)),
            (BipedObjectFlag.Amulet, FormKeys.SkyrimSE.Skyrim.Keyword.ClothingNecklace, nameof(FormKeys.SkyrimSE.Skyrim.Keyword.ClothingNecklace)),
            (BipedObjectFlag.Ring, FormKeys.SkyrimSE.Skyrim.Keyword.ClothingRing, nameof(FormKeys.SkyrimSE.Skyrim.Keyword.ClothingRing)),
        ]);

        foreach (var (slots, keyword, keywordEditorID) in conditions)
        {
            // Check if any of the slots are selected
            if ((armor.BodyTemplate.FirstPersonFlags & slots) != 0)
            {
                if (armor.Keywords.Contains(keyword))
                {
                    return null;
                }

                return new RecordAnalyzerResult(RecordTopic.Create(
                    armor,
                    ArmorMatchingKeywordSlots.Format(slots.ToString(), keywordEditorID),
                    x => x.Keywords));
            }
        }

        return null;
    }
}
