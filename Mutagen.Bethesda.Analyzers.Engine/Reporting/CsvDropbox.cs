using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins.Records;
namespace Mutagen.Bethesda.Analyzers.Reporting;

public class CsvDropbox : IReportDropbox
{
    private readonly IReportDropbox _dropbox;
    private readonly string _outputPath;

    public CsvDropbox(
        IReportDropbox dropbox,
        string outputPath)
    {
        _dropbox = dropbox;
        _outputPath = outputPath;
    }

    public void Dropoff(IModGetter sourceMod, IMajorRecordGetter majorRecord, ITopic topic)
    {
        _dropbox.Dropoff(sourceMod, majorRecord, topic);
        Append(BuildLine(topic, sourceMod, majorRecord));
    }

    public void Dropoff(ITopic topic)
    {
        _dropbox.Dropoff(topic);
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
        using var writer = new StreamWriter(_outputPath, true);
        writer.WriteLine(line);
    }
}
