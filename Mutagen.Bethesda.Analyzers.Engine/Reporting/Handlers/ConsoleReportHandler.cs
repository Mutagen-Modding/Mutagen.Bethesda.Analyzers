using Mutagen.Bethesda.Analyzers.SDK.Drops;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Reporting.Handlers;

public class ConsoleReportHandler : IReportHandler
{
    public void Dropoff(
        ReportContextParameters parameters,
        ModKey sourceMod,
        IMajorRecordIdentifier majorRecord,
        ITopic topic)
    {
        Console.WriteLine($"{topic.TopicDefinition}");
        Console.WriteLine($"   {sourceMod.ToString()} -> {majorRecord.FormKey.ToString()} {majorRecord.EditorID}");
        Console.WriteLine($"   {topic.FormattedTopic.FormattedMessage}");
    }

    public void Dropoff(
        ReportContextParameters parameters,
        ITopic topic)
    {
        Console.WriteLine($"{topic.TopicDefinition}");
        Console.WriteLine($"   {topic.FormattedTopic.FormattedMessage}");
    }
}
