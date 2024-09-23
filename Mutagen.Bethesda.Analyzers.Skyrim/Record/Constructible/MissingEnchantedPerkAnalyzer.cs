using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Constructible;

public class MissingEnchantedPerkAnalyzer : IIsolatedRecordAnalyzer<IConstructibleObjectGetter>
{
    public static readonly TopicDefinition MissingCreatedObject = MutagenTopicBuilder.DevelopmentTopic(
            "Missing Perk for Enchanted Weapon/Armor",
            Severity.Suggestion)
        .WithoutFormatting("Temper recipes need to have condition EPTemperingItemIsEnchanted = 0 or HasPerk ArcaneBlacksmith");

    public IEnumerable<TopicDefinition> Topics { get; } = [MissingCreatedObject];

    public RecordAnalyzerResult? AnalyzeRecord(IsolatedRecordAnalyzerParams<IConstructibleObjectGetter> param)
    {
        var constructibleObject = param.Record;

        // Only applicable to temper crafting stations for weapons and armor
        if (constructibleObject.WorkbenchKeyword.FormKey != FormKeys.SkyrimSE.Skyrim.Keyword.CraftingSmithingSharpeningWheel.FormKey
            && constructibleObject.WorkbenchKeyword.FormKey != FormKeys.SkyrimSE.Skyrim.Keyword.CraftingSmithingArmorTable.FormKey) return null;

        var perkCondition = false;
        var isEnchanted = false;
        foreach (var condition in constructibleObject.Conditions) {
            switch (condition.Data) {
                case IEPTemperingItemIsEnchantedConditionDataGetter: {
                    isEnchanted = true;
                    if (perkCondition) return null;

                    break;
                }
                case IHasPerkConditionDataGetter hasPerk when hasPerk.Perk.Link.FormKey == FormKeys.SkyrimSE.Skyrim.Perk.ArcaneBlacksmith.FormKey: {
                    perkCondition = true;
                    if (isEnchanted) return null;

                    break;
                }
            }

            if (!condition.Flags.HasFlag(Condition.Flag.OR)) {
                perkCondition = isEnchanted = false;
            }
        }

        return new RecordAnalyzerResult(
            RecordTopic.Create(
                obj: constructibleObject,
                formattedTopicDefinition: MissingCreatedObject.Format(),
                memberExpression: x => x.Conditions
            )
        );
    }
}
