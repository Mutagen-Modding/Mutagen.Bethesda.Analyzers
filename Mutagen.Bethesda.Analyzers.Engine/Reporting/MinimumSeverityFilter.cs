using Mutagen.Bethesda.Analyzers.Config;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Reporting;

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

    public void Dropoff(IModGetter sourceMod, IMajorRecordGetter majorRecord, ITopic topic)
    {
        if (topic.Severity < _minimum.MinimumSeverity) return;
        _reportDropbox.Dropoff(sourceMod, majorRecord, topic);
    }

    public void Dropoff(ITopic topic)
    {
        if (topic.Severity < _minimum.MinimumSeverity) return;
        _reportDropbox.Dropoff(topic);
    }
}
