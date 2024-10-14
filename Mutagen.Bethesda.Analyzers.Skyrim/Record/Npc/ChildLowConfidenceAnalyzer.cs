using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Npc;

public class ChildLowConfidenceAnalyzer : IIsolatedRecordsAnalyzer<INpcGetter, IRaceGetter>
{
    public static readonly TopicDefinition ChildLowConfidence = MutagenTopicBuilder.DevelopmentTopic(
            "Child low confidence",
            Severity.Suggestion)
        .WithoutFormatting("Child  doesn't have low confidence");

    public IEnumerable<TopicDefinition> Topics { get; } = [ChildLowConfidence];

    public void AnalyzeRecord(IsolatedRecordsAnalyzerParams<INpcGetter, IRaceGetter> param)
    {
        var npc = param.Record;
        if (!npc.Race.TryResolve(param.Lookup1, out var race)) return;
        if (!race.IsChildRace()) return;

        if (npc.AIData.Confidence is Confidence.Brave or Confidence.Foolhardy)
        {
            param.AddTopic(
                ChildLowConfidence.Format(),
                race);
        }
    }

    public IEnumerable<Func<INpcGetter, object?>> DriverFieldsOfInterest()
    {
        yield return x => x.MajorFlags;
        yield return x => x.Race;
    }

    public IEnumerable<Func<IRaceGetter, object?>> LookupFieldsOfInterest()
    {
        yield return x => x.Flags;
    }
}
