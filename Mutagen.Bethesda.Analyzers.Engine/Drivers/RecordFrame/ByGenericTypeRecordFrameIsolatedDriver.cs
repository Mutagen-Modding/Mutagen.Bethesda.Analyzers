using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Drops;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Binary.Headers;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.Plugins.Records.Mapping;

namespace Mutagen.Bethesda.Analyzers.Drivers.RecordFrame;

public class ByGenericTypeRecordFrameIsolatedDriver<TMajor> : IIsolatedRecordFrameAnalyzerDriver
    where TMajor : class, IMajorRecordGetter
{
    private readonly IIsolatedRecordFrameAnalyzer<TMajor>[] _isolatedRecordFrameAnalyzers;

    public bool Applicable => _isolatedRecordFrameAnalyzers.Length > 0;

    public IEnumerable<IAnalyzer> Analyzers => _isolatedRecordFrameAnalyzers;

    public RecordType TargetType => MajorRecordTypeLookup<TMajor>.RecordType;

    public ByGenericTypeRecordFrameIsolatedDriver(
        IIsolatedRecordFrameAnalyzer<TMajor>[] isolatedRecordFrameAnalyzers)
    {
        _isolatedRecordFrameAnalyzers = isolatedRecordFrameAnalyzers;
    }

    public void Drive(IsolatedDriverParams driverParams, MajorRecordFrame frame)
    {
        var reportContext = new ReportContextParameters(driverParams.LinkCache);
        var param = new IsolatedRecordFrameAnalyzerParams<TMajor>(
            driverParams.ReportDropbox,
            reportContext,
            frame);

        foreach (var analyzer in _isolatedRecordFrameAnalyzers)
        {
            analyzer.AnalyzeRecord(param with
            {
                AnalyzerType = analyzer.GetType()
            });
        }
    }
}
