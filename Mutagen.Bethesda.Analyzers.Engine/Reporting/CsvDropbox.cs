using Mutagen.Bethesda.Analyzers.Config;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins.Records;
namespace Mutagen.Bethesda.Analyzers.Reporting;

public class CsvDropbox : IReportDropbox
{
    private readonly IReportDropbox _dropbox;
    private readonly IReportOutputConfiguration _reportOutputConfiguration;

    public CsvDropbox(
        IReportDropbox dropbox,
        IReportOutputConfiguration reportOutputConfiguration)
    {
        _dropbox = dropbox;
        _reportOutputConfiguration = reportOutputConfiguration;
    }

    public void Dropoff(IModGetter sourceMod, IMajorRecordGetter majorRecord, ITopic topic)
    {
        Append(BuildLine(topic, sourceMod, majorRecord));
        _dropbox.Dropoff(sourceMod, majorRecord, topic);
    }

    public void Dropoff(ITopic topic)
    {
        Append(BuildLine(topic, null, null));
        _dropbox.Dropoff(topic);
    }

    private static string BuildLine(ITopic topic, IModGetter? sourceMod, IMajorRecordGetter? majorRecord)
    {
        return $"""
        "{topic.TopicDefinition.Id}","{topic.TopicDefinition.Severity}","{topic.TopicDefinition.Title}","{sourceMod?.ModKey.ToString()}","{majorRecord?.FormKey.ToString()}","{majorRecord?.EditorID}","{topic.FormattedMessage}"
        """;
    }

    private void Append(string line)
    {
        using var writer = new StreamWriter(_reportOutputConfiguration.OutputFilePath, true);
        writer.WriteLine(line);
    }
}
