using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Drivers
{
    public class ByTypeDriver<TMajor> : IDriver
        where TMajor : class, IMajorRecordGetter
    {
        private readonly IContextualRecordAnalyzer<TMajor>[] _contextualRecordAnalyzers;
        private readonly IIsolatedRecordAnalyzer<TMajor>[] _isolatedRecordAnalyzers;

        public bool Applicable => _isolatedRecordAnalyzers.Length > 0;

        public ByTypeDriver(
            IContextualRecordAnalyzer<TMajor>[] contextualRecordAnalyzers,
            IIsolatedRecordAnalyzer<TMajor>[] isolatedRecordAnalyzers)
        {
            _contextualRecordAnalyzers = contextualRecordAnalyzers;
            _isolatedRecordAnalyzers = isolatedRecordAnalyzers;
        }

        public void Drive(DriverParams driverParams)
        {
            var contextualParam = new ContextualRecordAnalyzerParams<TMajor>(
                driverParams.LinkCache,
                driverParams.LoadOrder);

            foreach (var rec in driverParams.TargetMod.EnumerateMajorRecords<TMajor>())
            {
                var isolatedParam = new IsolatedRecordAnalyzerParams<TMajor>(rec);
                foreach (var analyzer in _isolatedRecordAnalyzers)
                {
                    driverParams.ReportDropbox.Dropoff(driverParams.TargetMod, rec, analyzer.AnalyzeRecord(isolatedParam));
                }

                contextualParam = contextualParam with { Record = rec };
                foreach (var analyzer in _contextualRecordAnalyzers)
                {
                    driverParams.ReportDropbox.Dropoff(driverParams.TargetMod, rec, analyzer.AnalyzeRecord(contextualParam));
                }
            }
        }
    }
}
