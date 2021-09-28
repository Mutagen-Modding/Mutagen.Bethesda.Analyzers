using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Drivers
{
    public class ByTypeDriver<TMajor> : IIsolatedDriver, IContextualDriver
        where TMajor : class, IMajorRecordGetter
    {
        private readonly IIsolatedRecordAnalyzer<TMajor>[] _isolatedRecordAnalyzers;
        private readonly IContextualRecordAnalyzer<TMajor>[] _contextualRecordAnalyzers;

        public bool Applicable => _isolatedRecordAnalyzers.Length > 0;

        public ByTypeDriver(
            IIsolatedRecordAnalyzer<TMajor>[] isolatedRecordAnalyzers,
            IContextualRecordAnalyzer<TMajor>[] contextualRecordAnalyzers)
        {
            _isolatedRecordAnalyzers = isolatedRecordAnalyzers;
            _contextualRecordAnalyzers = contextualRecordAnalyzers;
        }

        public void Drive(IIsolatedDriverParams driverParams)
        {
            foreach (var rec in driverParams.TargetMod.EnumerateMajorRecords<TMajor>())
            {
                var isolatedParam = new IsolatedRecordAnalyzerParams<TMajor>(rec);
                foreach (var analyzer in _isolatedRecordAnalyzers)
                {
                    driverParams.ReportDropbox.Dropoff(
                        driverParams.TargetMod,
                        rec,
                        analyzer.AnalyzeRecord(isolatedParam));
                }
            }
        }

        public void Drive(IContextualDriverParams driverParams)
        {
            var analyzerParams = new ContextualRecordAnalyzerParams<TMajor>(
                driverParams.LinkCache,
                driverParams.LoadOrder);

            foreach (var listing in driverParams.LoadOrder.ListedOrder)
            {
                if (listing.Mod == null) continue;

                foreach (var rec in listing.Mod.EnumerateMajorRecords<TMajor>())
                {
                    analyzerParams = analyzerParams with { Record = rec };
                    foreach (var analyzer in _contextualRecordAnalyzers)
                    {
                        driverParams.ReportDropbox.Dropoff(
                            listing.Mod,
                            rec,
                            analyzer.AnalyzeRecord(analyzerParams));
                    }
                }
            }
        }
    }
}
