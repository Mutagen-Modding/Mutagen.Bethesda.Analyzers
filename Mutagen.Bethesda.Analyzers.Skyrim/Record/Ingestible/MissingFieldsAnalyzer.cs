using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Ingestible;

public class MissingFieldsAnalyzer : IIsolatedRecordAnalyzer<IIngestibleGetter>
{
    public static readonly TopicDefinition EmptyEffectList = MutagenTopicBuilder.DevelopmentTopic(
            "Empty Effect List",
            Severity.Suggestion)
        .WithoutFormatting("Ingestible has no effect");

    public static readonly TopicDefinition NoConsumeSound = MutagenTopicBuilder.DevelopmentTopic(
            "No Consume Sound",
            Severity.Suggestion)
        .WithoutFormatting("Ingestible has no consume sound");

    public IEnumerable<TopicDefinition> Topics { get; } = [EmptyEffectList, NoConsumeSound];

    public void AnalyzeRecord(IsolatedRecordAnalyzerParams<IIngestibleGetter> param)
    {
        var ingestible = param.Record;

        if (ingestible.Effects.Count == 0)
        {
            param.AddTopic(
                EmptyEffectList.Format(),
                x => x.Effects);
        }

        if (ingestible.ConsumeSound.IsNull)
        {
            param.AddTopic(
                NoConsumeSound.Format(),
                x => x.ConsumeSound);
        }
    }
}
