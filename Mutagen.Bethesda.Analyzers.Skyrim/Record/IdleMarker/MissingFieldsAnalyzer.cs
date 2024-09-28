using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.IdleMarker;

public class MissingFieldsAnalyzer : IIsolatedRecordAnalyzer<IIdleMarkerGetter>
{
    public static readonly TopicDefinition NoIdles = MutagenTopicBuilder.DevelopmentTopic(
            "No Idles",
            Severity.Warning)
        .WithoutFormatting("Idle Marker has no animations");

    public IEnumerable<TopicDefinition> Topics { get; } = [NoIdles];

    public void AnalyzeRecord(IsolatedRecordAnalyzerParams<IIdleMarkerGetter> param)
    {
        var idleMarker = param.Record;

        if (idleMarker.Animations is not null && idleMarker.Animations.Count == 0)
        {
            param.AddTopic(
                NoIdles.Format(),
                x => x.Animations);
        }
    }
}
