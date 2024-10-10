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
        Topic topic)
    {
        Append(BuildLine(topic, mod, record));
    }

    public void Dropoff(
        ReportContextParameters parameters,
        Topic topic)
    {
        Append(BuildLine(topic, null, null));
    }

    private static string BuildLine(Topic topic, ModKey? sourceMod, IMajorRecordIdentifier? majorRecord)
    {
        if (topic.MetaData.Length > 0)
        {
            throw new NotImplementedException();
        }
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
