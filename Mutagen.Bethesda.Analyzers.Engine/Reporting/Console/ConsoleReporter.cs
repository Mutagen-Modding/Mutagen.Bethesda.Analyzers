using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Reporting.Console
{
    public class ConsoleReporter : IReportDropbox
    {
        public void Dropoff(
            IModGetter sourceMod,
            IMajorRecordCommonGetter majorRecord,
            RecordAnalyzerResult? result)
        {
            if (result == null || result.Topics.Count == 0) return;
            System.Console.WriteLine($"{sourceMod.ModKey.ToString()} -> {majorRecord.FormKey.ToString()}");
            foreach (var error in result.Topics)
            {
                System.Console.WriteLine($"  {error.FormattedTopicDefinition}");
            }
        }
    }
}
