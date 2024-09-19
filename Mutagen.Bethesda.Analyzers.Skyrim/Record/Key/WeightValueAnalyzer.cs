using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Key;

public class WeightValueAnalyzer : IIsolatedRecordAnalyzer<IKeyGetter>
{
    public static readonly TopicDefinition<float> WeightNotZero = MutagenTopicBuilder.DevelopmentTopic(
            "Weight Not Zero",
            Severity.Suggestion)
        .WithFormatting<float>("Key weight {0} is not zero");

    public static readonly TopicDefinition<uint> ValueNotZero = MutagenTopicBuilder.DevelopmentTopic(
            "Value Not Zero",
            Severity.Suggestion)
        .WithFormatting<uint>("Key value {0} is not zero");

    public IEnumerable<TopicDefinition> Topics { get; } = [WeightNotZero, ValueNotZero];

    public RecordAnalyzerResult AnalyzeRecord(IsolatedRecordAnalyzerParams<IKeyGetter> param)
    {
        var key = param.Record;

        var result = new RecordAnalyzerResult();

        if (key is not { Weight: 0 })
        {
            result.AddTopic(
                RecordTopic.Create(
                    key,
                    WeightNotZero.Format(key.Weight),
                    x => x.Weight
                )
            );
        }

        if (key.Value != 0)
        {
            result.AddTopic(
                RecordTopic.Create(
                    key,
                    ValueNotZero.Format(key.Value),
                    x => x.Value
                )
            );
        }

        return result;
    }
}
