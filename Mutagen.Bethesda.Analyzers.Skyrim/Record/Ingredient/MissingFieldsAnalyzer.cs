using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Ingredient;

public class MissingFieldsAnalyzer : IIsolatedRecordAnalyzer<IIngredientGetter>
{
    public static readonly TopicDefinition NoPickupSound = MutagenTopicBuilder.DevelopmentTopic(
            "No Pickup Sound",
            Severity.Suggestion)
        .WithoutFormatting("Ingredient has no pickup sound");

    public static readonly TopicDefinition NoPutDownSound = MutagenTopicBuilder.DevelopmentTopic(
            "No Put Down Sound",
            Severity.Suggestion)
        .WithoutFormatting("Ingredient has no put down sound");

    public static readonly TopicDefinition<int> NotFourEffects = MutagenTopicBuilder.DevelopmentTopic(
            "Not Four Effects",
            Severity.Suggestion)
        .WithFormatting<int>("Ingredient has {0} effects, not 4 as it is expected");

    public IEnumerable<TopicDefinition> Topics { get; } = [NoPickupSound, NoPutDownSound, NotFourEffects];

    public RecordAnalyzerResult AnalyzeRecord(IsolatedRecordAnalyzerParams<IIngredientGetter> param)
    {
        var ingredient = param.Record;

        var result = new RecordAnalyzerResult();

        if (ingredient.PickUpSound.IsNull)
        {
            result.AddTopic(
                RecordTopic.Create(
                    ingredient,
                    NoPickupSound.Format(),
                    x => x.PickUpSound
                )
            );
        }

        if (ingredient.PutDownSound.IsNull)
        {
            result.AddTopic(
                RecordTopic.Create(
                    ingredient,
                    NoPutDownSound.Format(),
                    x => x.PutDownSound
                )
            );
        }

        if (ingredient.Effects.Count != 4)
        {
            result.AddTopic(
                RecordTopic.Create(
                    ingredient,
                    NotFourEffects.Format(ingredient.Effects.Count),
                    x => x.Effects
                )
            );
        }

        return result;
    }
}
