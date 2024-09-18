using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Noggog;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Armor;

public class VendorKeywordAnalyzer : IIsolatedRecordAnalyzer<IArmorGetter>
{
    public static readonly TopicDefinition<string, string> ArmorMissingVendorKeyword = MutagenTopicBuilder.FromDiscussion(
            0,
            "Armor is missing Vendor Keyword",
            Severity.Suggestion)
        .WithFormatting<string, string>("Missing vendor keyword {0} but has {1}");

    public IEnumerable<TopicDefinition> Topics => [ArmorMissingVendorKeyword];

    private sealed record NamedFormLink(FormLink<IKeywordGetter> FormLink, string EditorID);
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
        if (armor.Keywords.Contains(FormKeys.SkyrimSE.Skyrim.Keyword.VendorItemDaedricArtifact)
            || armor.Keywords.Contains(FormKeys.SkyrimSE.Skyrim.Keyword.VendorNoSale)
            || armor.Keywords.Contains(FormKeys.SkyrimSE.Skyrim.Keyword.VendorItemArmor))
        {
            return null;
        }

        // Determine the expected vendor keyword based on the armor type
        var expectedVendorKeyword = armor.BodyTemplate?.ArmorType switch
        {
            ArmorType.Clothing or ArmorType.LightArmor when
                armor.Keywords.Contains(FormKeys.SkyrimSE.Skyrim.Keyword.ClothingCirclet)
                || armor.Keywords.Contains(FormKeys.SkyrimSE.Skyrim.Keyword.ClothingNecklace)
                || armor.Keywords.Contains(FormKeys.SkyrimSE.Skyrim.Keyword.ClothingRing)
                || armor.Keywords.Contains(FormKeys.SkyrimSE.Skyrim.Keyword.ArmorJewelry)
                || armor.Keywords.Contains(FormKeys.SkyrimSE.Skyrim.Keyword.JewelryExpensive)
                => new NamedFormLink(FormKeys.SkyrimSE.Skyrim.Keyword.VendorItemJewelry, nameof(FormKeys.SkyrimSE.Skyrim.Keyword.VendorItemJewelry)),
            ArmorType.Clothing
                => new NamedFormLink(FormKeys.SkyrimSE.Skyrim.Keyword.VendorItemClothing, nameof(FormKeys.SkyrimSE.Skyrim.Keyword.VendorItemClothing)),
            _
                => new NamedFormLink(FormKeys.SkyrimSE.Skyrim.Keyword.VendorItemArmor, nameof(FormKeys.SkyrimSE.Skyrim.Keyword.VendorItemArmor))
        };

        if (armor.Keywords.Contains(expectedVendorKeyword.FormLink)) return null;

        var foundVendorKeywordNames = armor.Keywords
            .Select(x =>
            {
                if (x.FormKey == FormKeys.SkyrimSE.Skyrim.Keyword.VendorItemArmor.FormKey)
                {
                    return new NamedFormLink(FormKeys.SkyrimSE.Skyrim.Keyword.VendorItemArmor, nameof(FormKeys.SkyrimSE.Skyrim.Keyword.VendorItemArmor));
                }

                if (x.FormKey == FormKeys.SkyrimSE.Skyrim.Keyword.VendorItemClothing.FormKey)
                {
                    return new NamedFormLink(FormKeys.SkyrimSE.Skyrim.Keyword.VendorItemClothing, nameof(FormKeys.SkyrimSE.Skyrim.Keyword.VendorItemClothing));
                }

                if (x.FormKey == FormKeys.SkyrimSE.Skyrim.Keyword.VendorItemJewelry.FormKey)
                {
                    return new NamedFormLink(FormKeys.SkyrimSE.Skyrim.Keyword.VendorItemJewelry, nameof(FormKeys.SkyrimSE.Skyrim.Keyword.VendorItemJewelry));
                }

                return null;
            })
            .NotNull()
            .ToList();

        var foundVendorKeywords = foundVendorKeywordNames.Count == 0 ? "None" : string.Join(", ", foundVendorKeywordNames.Select(x => x.EditorID));

        return new RecordAnalyzerResult(
            RecordTopic.Create(
                armor,
                ArmorMissingVendorKeyword.Format($"{expectedVendorKeyword.EditorID}", foundVendorKeywords),
                x => x.Keywords
                // additional data to be used by fixer in the future
                // new Dictionary<string, object>
                // {
                //     { "ExpectedVendorKeyword", expectedVendorKeyword },
                //     { "FoundVendorKeywords", foundVendorKeywords }
                // }
            )
        );
    }
}
