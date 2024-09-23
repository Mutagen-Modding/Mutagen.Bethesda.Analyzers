using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Spell;

public class MissingFieldsAnalyzer : IIsolatedRecordAnalyzer<ISpellGetter>
{
    public static readonly TopicDefinition EmptyEffectList = MutagenTopicBuilder.DevelopmentTopic(
            "Empty Effect List",
            Severity.Suggestion)
        .WithoutFormatting("Spell has no effect");

    public IEnumerable<TopicDefinition> Topics { get; } = [EmptyEffectList];

    public RecordAnalyzerResult AnalyzeRecord(IsolatedRecordAnalyzerParams<ISpellGetter> param)
    {
        var spell = param.Record;

        var result = new RecordAnalyzerResult();

        if (spell.Effects.Count == 0)
        {
            result.AddTopic(
                RecordTopic.Create(
                    spell,
                    EmptyEffectList.Format(),
                    x => x.Effects
                )
            );
        }

        return result;
    }
}
