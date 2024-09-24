using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Reporting.Handlers;

public record CsvInputs(string OutputFilePath);

public class CsvReportHandler : IReportHandler
{
    private readonly CsvInputs _inputs;

    public CsvReportHandler(
        CsvInputs inputs)
    {
        _inputs = inputs;
    }

    public void Dropoff(
        HandlerParameters parameters,
        IModGetter sourceMod,
        IMajorRecordGetter majorRecord,
        ITopic topic)
    {
        Append(BuildLine(topic, sourceMod, majorRecord));
    }

    public void Dropoff(
        HandlerParameters parameters,
        ITopic topic)
    {
        Append(BuildLine(topic, null, null));
    }

    private static string BuildLine(ITopic topic, IModGetter? sourceMod, IMajorRecordGetter? majorRecord)
    {
        return $"""
        "{topic.TopicDefinition.Id}","{topic.TopicDefinition.Severity}","{topic.TopicDefinition.Title}","{sourceMod?.ModKey.ToString()}","{majorRecord?.FormKey.ToString()}","{majorRecord?.EditorID}","{topic.FormattedMessage}"
        """;
    }

    private void Append(string line)
    {
        using var writer = new StreamWriter(_inputs.OutputFilePath, true);
        writer.WriteLine(line);
    }
}
