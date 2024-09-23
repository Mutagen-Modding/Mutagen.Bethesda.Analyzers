using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Cell.Interior;

public class MissingFieldsAnalyzer : IIsolatedRecordAnalyzer<ICellGetter>
{
    public static readonly TopicDefinition NoMusic = MutagenTopicBuilder.DevelopmentTopic(
            "No Music",
            Severity.Suggestion)
        .WithoutFormatting("Interior cell has no music");

    public static readonly TopicDefinition NoLightingTemplate = MutagenTopicBuilder.DevelopmentTopic(
            "No Lighting Template",
            Severity.Suggestion)
        .WithoutFormatting("Interior cell has no lighting template");

    public static readonly TopicDefinition NoAcousticSpace = MutagenTopicBuilder.DevelopmentTopic(
            "No Acoustic Space",
            Severity.Suggestion)
        .WithoutFormatting("Interior cell has no acoustic space");

    public static readonly TopicDefinition NoLocation = MutagenTopicBuilder.DevelopmentTopic(
            "No Location",
            Severity.Suggestion)
        .WithoutFormatting("Interior cell has no location");

    public IEnumerable<TopicDefinition> Topics { get; } = [NoMusic, NoLightingTemplate, NoAcousticSpace, NoLocation];

    public RecordAnalyzerResult? AnalyzeRecord(IsolatedRecordAnalyzerParams<ICellGetter> param)
    {
        var cell = param.Record;
        if (cell.IsExteriorCell()) return null;

        var result = new RecordAnalyzerResult();

        if (cell.Music.IsNull)
        {
            result.AddTopic(
                RecordTopic.Create(
                    cell,
                    NoMusic.Format(),
                    x => x.Music
                )
            );
        }

        if (cell.LightingTemplate.IsNull)
        {
            result.AddTopic(
                RecordTopic.Create(
                    cell,
                    NoLightingTemplate.Format(),
                    x => x.LightingTemplate
                )
            );
        }

        if (cell.AcousticSpace.IsNull)
        {
            result.AddTopic(
                RecordTopic.Create(
                    cell,
                    NoLightingTemplate.Format(),
                    x => x.LightingTemplate
                )
            );
        }

        if (cell.Location.IsNull)
        {
            result.AddTopic(
                RecordTopic.Create(
                    cell,
                    NoLightingTemplate.Format(),
                    x => x.LightingTemplate
                )
            );
        }

        return result;
    }
}
