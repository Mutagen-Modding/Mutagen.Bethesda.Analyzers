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
        Topic topic)
    {
        Console.WriteLine($"{topic.TopicDefinition}");
        Console.WriteLine($"   {sourceMod.ToString()} -> {majorRecord.FormKey.ToString()} {majorRecord.EditorID}");
        Console.WriteLine($"   {topic.FormattedTopic.FormattedMessage}");

        PrintMetadata(topic);
    }

    public void Dropoff(
        ReportContextParameters parameters,
        Topic topic)
    {
        Console.WriteLine($"{topic.TopicDefinition}");
        Console.WriteLine($"   {topic.FormattedTopic.FormattedMessage}");
    }

    private void PrintMetadata(Topic topic)
    {
        // ToDo
        // Handle various types like dictionary better
        // Also have a parameter to omit these, optionally

        foreach (var meta in topic.MetaData)
        {
            Console.WriteLine($"{meta.Name}: {meta.Value}");
        }
    }
}
