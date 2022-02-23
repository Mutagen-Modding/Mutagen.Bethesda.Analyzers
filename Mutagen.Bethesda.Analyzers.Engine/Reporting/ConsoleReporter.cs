using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Reporting;

public class ConsoleReporter : IReportDropbox
{
    public void Dropoff(IModGetter sourceMod, IMajorRecordGetter majorRecord, ITopic topic)
    {
        System.Console.WriteLine($"{topic.TopicDefinition}");
        System.Console.WriteLine($"   {sourceMod.ModKey.ToString()} -> {majorRecord.FormKey.ToString()}");
        System.Console.WriteLine($"   {topic.FormattedMessage}");
    }

    public void Dropoff(ITopic topic)
    {
        System.Console.WriteLine($"{topic.TopicDefinition}");
        System.Console.WriteLine($"   {topic.FormattedMessage}");
    }
}
