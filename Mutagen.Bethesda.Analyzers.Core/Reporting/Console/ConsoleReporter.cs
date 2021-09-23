using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Reporting.Console
{
    public class ConsoleReporter : IReportDropbox
    {
        public void Dropoff(
            IModGetter sourceMod,
            IMajorRecordCommonGetter majorRecord,
            MajorRecordAnalyzerResult? result)
        {
            if (result == null || result.Errors.Count == 0) return;
            System.Console.WriteLine($"{sourceMod.ModKey} -> {majorRecord.FormKey}");
            foreach (var error in result.Errors)
            {
                System.Console.WriteLine($"  {error.ErrorDefinition}");
            }
        }
    }
}
