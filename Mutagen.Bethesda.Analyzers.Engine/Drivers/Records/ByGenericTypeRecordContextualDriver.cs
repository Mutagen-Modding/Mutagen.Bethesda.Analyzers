using Mutagen.Bethesda.Analyzers.Reporting;
using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Drivers.Records;

public class ByGenericTypeRecordContextualDriver<TMajor> : IContextualDriver
    where TMajor : class, IMajorRecordGetter
{
    private readonly IContextualRecordAnalyzer<TMajor>[] _contextualRecordAnalyzers;

    public bool Applicable => _contextualRecordAnalyzers.Length > 0;

    public IEnumerable<IAnalyzer> Analyzers => _contextualRecordAnalyzers;

    public ByGenericTypeRecordContextualDriver(
        IContextualRecordAnalyzer<TMajor>[] contextualRecordAnalyzers)
    {
        _contextualRecordAnalyzers = contextualRecordAnalyzers;
    }

    public void Drive(ContextualDriverParams driverParams)
    {
        var reportContext = new ReportContextParameters(driverParams.LinkCache);

        foreach (var listing in driverParams.LoadOrder.ListedOrder)
        {
            if (listing.Mod is null) continue;

            foreach (var rec in listing.Mod.EnumerateMajorRecords<TMajor>())
            {
                foreach (var analyzer in _contextualRecordAnalyzers)
                {
                    var result = analyzer.AnalyzeRecord(new ContextualRecordAnalyzerParams<TMajor>(
                        driverParams.LinkCache,
                        driverParams.LoadOrder,
                        rec));
                    if (result is null) continue;
                    foreach (var topic in result.Topics)
                    {
                        driverParams.ReportDropbox.Dropoff(
                            reportContext,
                            listing.Mod,
                            rec,
                            topic);
                    }
                }
            }
        }
    }
}
