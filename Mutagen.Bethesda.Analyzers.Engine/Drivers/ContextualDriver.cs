using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Drops;

namespace Mutagen.Bethesda.Analyzers.Drivers;

public class ContextualDriver : IContextualDriver
{
    private readonly IContextualAnalyzer[] _contextualAnalyzers;
    public bool Applicable => true;
    public IEnumerable<IAnalyzer> Analyzers => _contextualAnalyzers;

    public ContextualDriver(IContextualAnalyzer[] contextualAnalyzers)
    {
        _contextualAnalyzers = contextualAnalyzers;
    }

    public void Drive(ContextualDriverParams driverParams)
    {
        var reportContext = new ReportContextParameters(driverParams.LinkCache);
        var analyzerParams = new ContextualAnalyzerParams(
            driverParams.LinkCache,
            driverParams.LoadOrder,
            driverParams.ReportDropbox,
            reportContext);
        foreach (var contextualAnalyzer in _contextualAnalyzers)
        {
            contextualAnalyzer.Analyze(analyzerParams with
            {
                AnalyzerType = contextualAnalyzer.GetType()
            });
        }
    }
}
