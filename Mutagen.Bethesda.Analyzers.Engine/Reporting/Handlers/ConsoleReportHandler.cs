using Mutagen.Bethesda.Analyzers.SDK.Drops;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Records;
using Noggog.WorkEngine;

namespace Mutagen.Bethesda.Analyzers.Reporting.Handlers;

public class ConsoleReportHandler : IReportHandler
{
    private readonly IWorkDropoff _workDropoff;

    public ConsoleReportHandler(IWorkDropoff workDropoff)
    {
        _workDropoff = workDropoff;
    }

    public void Dropoff(
        ReportContextParameters parameters,
        ModKey sourceMod,
        IMajorRecordIdentifierGetter majorRecord,
        Topic topic)
    {
        _workDropoff.Enqueue(() =>
        {
            Console.WriteLine($"{topic.TopicDefinition}");
            Console.WriteLine($"   {sourceMod.ToString()} -> {majorRecord.FormKey.ToString()} {majorRecord.EditorID}");
            Console.WriteLine($"   {topic.FormattedTopic.FormattedMessage}");

            PrintMetadata(topic);
        });
    }

    public void Dropoff(
        ReportContextParameters parameters,
        Topic topic)
    {
        _workDropoff.Enqueue(() =>
        {
            Console.WriteLine($"{topic.TopicDefinition}");
            Console.WriteLine($"   {topic.FormattedTopic.FormattedMessage}");
        });
    }

    private static void PrintMetadata(Topic topic)
    {
        // ToDo
        // Also have a parameter to omit these, optionally

        foreach (var meta in topic.MetaData)
        {
            Console.WriteLine($"{ReportUtility.GetStringValue(meta.Name)}: {ReportUtility.GetStringValue(meta.Value)}");
        }
    }
}
