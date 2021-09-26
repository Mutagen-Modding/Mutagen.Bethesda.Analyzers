using Mutagen.Bethesda.Analyzers.Reporting;
using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Drivers
{
    public class MajorRecordDriver<TMajor> : IModDriver
        where TMajor : class, IMajorRecordGetter
    {
        private readonly IIsolatedRecordAnalyzer<TMajor>[] _analyzers;

        public bool Applicable => _analyzers.Length > 0;

        public MajorRecordDriver(IIsolatedRecordAnalyzer<TMajor>[] analyzers)
        {
            _analyzers = analyzers;
        }

        public void Drive(IModGetter modGetter, IReportDropbox dropbox)
        {
            foreach (var rec in modGetter.EnumerateMajorRecords<TMajor>())
            {
                var param = new IsolatedRecordAnalyzerParams<TMajor>(rec);
                foreach (var analyzer in _analyzers)
                {
                    dropbox.Dropoff(modGetter, rec, analyzer.AnalyzeRecord(param));
                }
            }
        }
    }
}
