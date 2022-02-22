using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Reporting.Console
{
    public class ConsoleReporter : IReportDropbox
    {
        public void Dropoff<TError>(IModGetter sourceMod, IMajorRecordGetter majorRecord, IAnalyzerResult<TError> result) where TError : ITopic
        {
            if (result.Topics.Count == 0) return;
            foreach (var error in result.Topics)
            {
                System.Console.WriteLine($"{error.FormattedTopicDefinition.TopicDefinition}");
                System.Console.WriteLine($"   {sourceMod.ModKey.ToString()} -> {majorRecord.FormKey.ToString()}");
                System.Console.WriteLine($"   {error.FormattedTopicDefinition}");
            }
        }

        public void Dropoff<TError>(IAnalyzerResult<TError> result) where TError : ITopic
        {
            if (result.Topics.Count == 0) return;
            foreach (var error in result.Topics)
            {
                System.Console.WriteLine($"{error.FormattedTopicDefinition.TopicDefinition}");
                System.Console.WriteLine($"   {error.FormattedTopicDefinition}");
            }
        }
    }
}
