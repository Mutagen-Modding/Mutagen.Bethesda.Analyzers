using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Drops;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Drivers.Records;

public class ByGenericTypeRecordIsolatedDriver<TMajor> : IIsolatedDriver
    where TMajor : class, IMajorRecordGetter
{
    private readonly IIsolatedRecordAnalyzer<TMajor>[] _isolatedRecordAnalyzers;

    public bool Applicable => _isolatedRecordAnalyzers.Length > 0;

    public IEnumerable<IAnalyzer> Analyzers => _isolatedRecordAnalyzers;

    public ByGenericTypeRecordIsolatedDriver(
        IIsolatedRecordAnalyzer<TMajor>[] isolatedRecordAnalyzers)
    {
        _isolatedRecordAnalyzers = isolatedRecordAnalyzers;
    }

    public void Drive(IsolatedDriverParams driverParams)
    {
        var reportContext = new ReportContextParameters(driverParams.LinkCache);

        foreach (var rec in driverParams.TargetMod.EnumerateMajorRecords<TMajor>())
        {
            var isolatedParam = new IsolatedRecordAnalyzerParams<TMajor>(
                driverParams.TargetMod.ModKey,
                rec,
                reportContext,
                driverParams.ReportDropbox);
            foreach (var analyzer in _isolatedRecordAnalyzers)
            {
                analyzer.AnalyzeRecord(isolatedParam);
            }
        }
    }
}
