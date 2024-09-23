using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Npc;

public class ChildLowConfidenceAnalyzer : IContextualRecordAnalyzer<INpcGetter>
{
    public static readonly TopicDefinition ChildLowConfidence = MutagenTopicBuilder.DevelopmentTopic(
            "Child low confidence",
            Severity.Suggestion)
        .WithoutFormatting("Child  doesn't have low confidence");

    public IEnumerable<TopicDefinition> Topics { get; } = [ChildLowConfidence];

    public RecordAnalyzerResult? AnalyzeRecord(ContextualRecordAnalyzerParams<INpcGetter> param)
    {
        var npc = param.Record;
        if (!npc.Race.TryResolve(param.LinkCache, out var race)) return null;
        if (!race.IsChildRace()) return null;

        if (npc.AIData.Confidence is Confidence.Brave or Confidence.Foolhardy)
        {
            return new RecordAnalyzerResult(
                RecordTopic.Create(
                    npc,
                    ChildLowConfidence.Format(),
                    x => x.AIData));
        }

        return null;
    }
}
