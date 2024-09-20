using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Armor;

public class VendorKeywordAnalyzer : IIsolatedRecordAnalyzer<IArmorGetter>
{
    public static readonly TopicDefinition<FormLink<IKeywordGetter>> ArmorMissingVendorKeyword = MutagenTopicBuilder.DevelopmentTopic(
            "Armor is missing Vendor Keyword",
            Severity.Suggestion)
        .WithFormatting<FormLink<IKeywordGetter>>("Missing vendor keyword {0}");

    public static readonly TopicDefinition<FormLink<IKeywordGetter>, List<IFormLinkGetter<IKeywordGetter>>> UnsuitableVendorKeyword = MutagenTopicBuilder.DevelopmentTopic(
            "Armor has unsuitable Vendor Keyword",
            Severity.Suggestion)
        .WithFormatting<FormLink<IKeywordGetter>, List<IFormLinkGetter<IKeywordGetter>>>("Expected vendor keyword {0}, found {1}");

    public IEnumerable<TopicDefinition> Topics => [ArmorMissingVendorKeyword];

    private static readonly HashSet<FormLink<IKeywordGetter>> AllArmorVendorKeywords =
    [
        FormKeys.SkyrimSE.Skyrim.Keyword.VendorItemDaedricArtifact,
        FormKeys.SkyrimSE.Skyrim.Keyword.VendorNoSale,
        FormKeys.SkyrimSE.Skyrim.Keyword.VendorItemArmor,
        FormKeys.SkyrimSE.Skyrim.Keyword.VendorItemClothing,
        FormKeys.SkyrimSE.Skyrim.Keyword.VendorItemJewelry,
    ];

    private static readonly HashSet<FormLink<IKeywordGetter>> AlwaysValidVendorKeywords =
    [
        FormKeys.SkyrimSE.Skyrim.Keyword.VendorItemDaedricArtifact,
        FormKeys.SkyrimSE.Skyrim.Keyword.VendorNoSale,
        FormKeys.SkyrimSE.Skyrim.Keyword.VendorItemArmor,
    ];

    private static readonly HashSet<FormLink<IKeywordGetter>> JeweleryKeywords =
    [
        FormKeys.SkyrimSE.Skyrim.Keyword.ClothingCirclet,
        FormKeys.SkyrimSE.Skyrim.Keyword.ClothingNecklace,
        FormKeys.SkyrimSE.Skyrim.Keyword.ClothingRing,
        FormKeys.SkyrimSE.Skyrim.Keyword.ArmorJewelry,
        FormKeys.SkyrimSE.Skyrim.Keyword.JewelryExpensive,
    ];

    public RecordAnalyzerResult? AnalyzeRecord(IsolatedRecordAnalyzerParams<IArmorGetter> param)
    {
        var armor = param.Record;

        // Armor with template armor inherit all relevant data from the template armor and should not be checked themselves
        if (!armor.TemplateArmor.IsNull) return null;

        // Non-playable armor should not have vendor keywords
        if (armor.MajorFlags.HasFlag(Bethesda.Skyrim.Armor.MajorFlag.NonPlayable)) return null;

        // Ignore armor with no keywords, these are usually skin armor
        if (armor.Keywords is null) return null;

        // No sale or Daedric artifact or the default armor vendor keywords are always allowed
        if (armor.Keywords.Intersect(AlwaysValidVendorKeywords).Any())
        {
            return null;
        }

        // Determine the expected vendor keyword based on the armor type
        var expectedVendorKeyword = armor.BodyTemplate?.ArmorType switch
        {
            ArmorType.Clothing or ArmorType.LightArmor when armor.Keywords.Intersect(JeweleryKeywords).Any()
                => FormKeys.SkyrimSE.Skyrim.Keyword.VendorItemJewelry,
            ArmorType.Clothing
                => FormKeys.SkyrimSE.Skyrim.Keyword.VendorItemClothing,
            _
                => FormKeys.SkyrimSE.Skyrim.Keyword.VendorItemArmor,
        };

        if (armor.Keywords.Contains(expectedVendorKeyword)) return null;

        // Collect all vendor keywords - at this point we know that these are all not the vendor keywords we are looking for
        var vendorKeywords = armor.Keywords.Intersect(AllArmorVendorKeywords).ToList();

        if (vendorKeywords.Count == 0)
        {
            return new RecordAnalyzerResult(
                RecordTopic.Create(
                    armor,
                    ArmorMissingVendorKeyword.Format(expectedVendorKeyword),
                    x => x.Keywords
                )
            );
        }

        return new RecordAnalyzerResult(
            RecordTopic.Create(
                armor,
                UnsuitableVendorKeyword.Format(expectedVendorKeyword, vendorKeywords),
                x => x.Keywords
            )
        );
    }
}
