using Mutagen.Bethesda.Analyzers.SDK.Drops;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins;
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
        ReportContextParameters parameters,
        ModKey mod,
        IMajorRecordIdentifier record,
        ITopic topic)
    {
        Append(BuildLine(topic, mod, record));
    }

    public void Dropoff(
        ReportContextParameters parameters,
        ITopic topic)
    {
        Append(BuildLine(topic, null, null));
    }

    private static string BuildLine(ITopic topic, ModKey? sourceMod, IMajorRecordIdentifier? majorRecord)
    {
        return $"""
        "{topic.TopicDefinition.Id}","{topic.TopicDefinition.Severity}","{topic.TopicDefinition.Title}","{sourceMod?.ToString()}","{majorRecord?.FormKey.ToString()}","{majorRecord?.EditorID}","{topic.FormattedTopic.FormattedMessage}"
        """;
    }

    private void Append(string line)
    {
        using var writer = new StreamWriter(_inputs.OutputFilePath, true);
        writer.WriteLine(line);
    }
}
