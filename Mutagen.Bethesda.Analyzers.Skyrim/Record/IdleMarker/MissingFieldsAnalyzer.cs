using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
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

    public RecordAnalyzerResult AnalyzeRecord(IsolatedRecordAnalyzerParams<IIdleMarkerGetter> param)
    {
        var idleMarker = param.Record;

        var result = new RecordAnalyzerResult();

        if (idleMarker.Animations is not null && idleMarker.Animations.Any())
        {
            result.AddTopic(
                RecordTopic.Create(
                    idleMarker,
                    NoIdles.Format(),
                    x => x.Animations
                )
            );
        }

        return result;
    }
}
