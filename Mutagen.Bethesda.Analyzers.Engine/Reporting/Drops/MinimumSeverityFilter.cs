using Mutagen.Bethesda.Analyzers.Config;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Reporting.Drops;

public class MinimumSeverityFilter : IReportDropbox
{
    private readonly IMinimumSeverityConfiguration _minimum;
    private readonly IReportDropbox _reportDropbox;

    public MinimumSeverityFilter(
        IMinimumSeverityConfiguration minimum,
        IReportDropbox reportDropbox)
    {
        _minimum = minimum;
        _reportDropbox = reportDropbox;
    }

    public void Dropoff(
        ReportContextParameters parameters,
        IModGetter sourceMod,
        IMajorRecordGetter majorRecord,
        ITopic topic)
    {
        if (topic.Severity < _minimum.MinimumSeverity) return;
        _reportDropbox.Dropoff(parameters, sourceMod, majorRecord, topic);
    }

    public void Dropoff(
        ReportContextParameters parameters,
        ITopic topic)
    {
        if (topic.Severity < _minimum.MinimumSeverity) return;
        _reportDropbox.Dropoff(parameters, topic);
    }
}
