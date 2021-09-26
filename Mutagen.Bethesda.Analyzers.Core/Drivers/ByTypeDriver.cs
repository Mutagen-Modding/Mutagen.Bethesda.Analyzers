using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Drivers
{
    public class ByTypeDriver<TMajor> : IDriver
        where TMajor : class, IMajorRecordGetter
    {
        private readonly IIsolatedRecordAnalyzer<TMajor>[] _recordAnalyzers;

        public bool Applicable => _recordAnalyzers.Length > 0;

        public ByTypeDriver(
            IIsolatedRecordAnalyzer<TMajor>[] recordAnalyzers)
        {
            _recordAnalyzers = recordAnalyzers;
        }

        public void Drive(DriverParams driverParams)
        {
            var param = new RecordAnalyzerParams<TMajor>(
                driverParams.LinkCache,
                driverParams.LoadOrder);

            foreach (var rec in driverParams.TargetMod.EnumerateMajorRecords<TMajor>())
            {
                param = param with { Record = rec };
                foreach (var analyzer in _recordAnalyzers)
                {
                    driverParams.ReportDropbox.Dropoff(driverParams.TargetMod, rec, analyzer.AnalyzeRecord(param));
                }
            }
        }
    }
}
