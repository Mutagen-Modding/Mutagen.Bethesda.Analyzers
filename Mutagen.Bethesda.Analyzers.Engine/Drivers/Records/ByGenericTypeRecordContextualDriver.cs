using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Plugins.Records;
using Noggog.WorkEngine;

namespace Mutagen.Bethesda.Analyzers.Drivers.Records;

public class ByGenericTypeRecordContextualDriver<TMajor> : IContextualDriver
    where TMajor : class, IMajorRecordGetter
{
    private readonly IWorkDropoff _dropoff;
    private readonly IContextualRecordAnalyzer<TMajor>[] _contextualRecordAnalyzers;

    public bool Applicable => _contextualRecordAnalyzers.Length > 0;

    public IEnumerable<IAnalyzer> Analyzers => _contextualRecordAnalyzers;

    public ByGenericTypeRecordContextualDriver(
        IContextualRecordAnalyzer<TMajor>[] contextualRecordAnalyzers,
        IWorkDropoff dropoff)
    {
        _contextualRecordAnalyzers = contextualRecordAnalyzers;
        _dropoff = dropoff;
    }

    public async Task Drive(ContextualDriverParams driverParams)
    {
        if (_contextualRecordAnalyzers.Length == 0) return;
        foreach (var listing in driverParams.LoadOrder.ListedOrder)
        {
            if (listing.Mod is null) continue;

            foreach (var rec in listing.Mod.EnumerateMajorRecords<TMajor>())
            {
                var param = new ContextualRecordAnalyzerParams<TMajor>(
                    driverParams.LinkCache,
                    driverParams.LoadOrder,
                    rec,
                    driverParams.ReportDropbox);
                await Task.WhenAll(_contextualRecordAnalyzers.Select(analyzer =>
                {
                    return _dropoff.EnqueueAndWait(() =>
                    {
                        analyzer.AnalyzeRecord(param with
                        {
                            AnalyzerType = analyzer.GetType()
                        });
                    });
                }));
            }
        }
    }
}
