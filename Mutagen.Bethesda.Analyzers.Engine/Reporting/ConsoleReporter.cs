using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Reporting;

public class ConsoleReporter : IReportDropbox
{
    public void Dropoff(IModGetter sourceMod, IMajorRecordGetter majorRecord, ITopic topic)
    {
        Console.WriteLine($"{topic.TopicDefinition}");
        Console.WriteLine($"   {sourceMod.ModKey.ToString()} -> {majorRecord.FormKey.ToString()} {majorRecord.EditorID}");
        Console.WriteLine($"   {topic.FormattedMessage}");
    }

    public void Dropoff(ITopic topic)
    {
        Console.WriteLine($"{topic.TopicDefinition}");
        Console.WriteLine($"   {topic.FormattedMessage}");
    }
}
