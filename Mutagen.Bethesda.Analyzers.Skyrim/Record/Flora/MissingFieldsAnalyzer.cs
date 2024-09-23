using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Flora;

public class MissingFieldsAnalyzer : IIsolatedRecordAnalyzer<IFloraGetter>
{
    public static readonly TopicDefinition NoHarvestSound = MutagenTopicBuilder.DevelopmentTopic(
            "No Harvest Sound",
            Severity.Suggestion)
        .WithoutFormatting("Flora has no harvest sound");

    public static readonly TopicDefinition NoIngredient = MutagenTopicBuilder.DevelopmentTopic(
            "No Ingredient",
            Severity.Warning)
        .WithoutFormatting("Flora has no ingredient");

    public IEnumerable<TopicDefinition> Topics { get; } = [NoHarvestSound, NoIngredient];

    public RecordAnalyzerResult AnalyzeRecord(IsolatedRecordAnalyzerParams<IFloraGetter> param)
    {
        var flora = param.Record;

        var result = new RecordAnalyzerResult();

        if (flora.HarvestSound.IsNull)
        {
            result.AddTopic(
                RecordTopic.Create(
                    flora,
                    NoHarvestSound.Format(),
                    x => x.HarvestSound
                )
            );
        }

        if (flora.Ingredient.IsNull)
        {
            result.AddTopic(
                RecordTopic.Create(
                    flora,
                    NoIngredient.Format(),
                    x => x.Ingredient
                )
            );
        }

        return result;
    }
}
