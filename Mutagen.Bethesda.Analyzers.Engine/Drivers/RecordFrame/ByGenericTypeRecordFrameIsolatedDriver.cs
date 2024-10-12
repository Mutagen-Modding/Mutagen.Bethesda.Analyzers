using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Drops;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Binary.Headers;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.Plugins.Records.Mapping;
using Noggog.WorkEngine;

namespace Mutagen.Bethesda.Analyzers.Drivers.RecordFrame;

public class ByGenericTypeRecordFrameIsolatedDriver<TMajor> : IIsolatedRecordFrameAnalyzerDriver
    where TMajor : class, IMajorRecordGetter
{
    private readonly IWorkDropoff _dropoff;
    private readonly IIsolatedRecordFrameAnalyzer<TMajor>[] _isolatedRecordFrameAnalyzers;

    public bool Applicable => _isolatedRecordFrameAnalyzers.Length > 0;

    public IEnumerable<IAnalyzer> Analyzers => _isolatedRecordFrameAnalyzers;

    public RecordType TargetType => MajorRecordTypeLookup<TMajor>.RecordType;

    public ByGenericTypeRecordFrameIsolatedDriver(
        IIsolatedRecordFrameAnalyzer<TMajor>[] isolatedRecordFrameAnalyzers,
        IWorkDropoff dropoff)
    {
        _isolatedRecordFrameAnalyzers = isolatedRecordFrameAnalyzers;
        _dropoff = dropoff;
    }

    public async Task Drive(IsolatedDriverParams driverParams, MajorRecordFrame frame)
    {
        var reportContext = new ReportContextParameters(driverParams.LinkCache);
        var param = new IsolatedRecordFrameAnalyzerParams<TMajor>(
            driverParams.ReportDropbox,
            reportContext,
            frame);

        await Task.WhenAll(_isolatedRecordFrameAnalyzers.Select(analyzer =>
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
