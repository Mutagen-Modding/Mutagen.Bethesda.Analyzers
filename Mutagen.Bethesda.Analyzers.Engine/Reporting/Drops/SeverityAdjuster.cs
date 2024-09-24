using Mutagen.Bethesda.Analyzers.Config;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Reporting.Drops;

public class SeverityAdjuster : IReportDropbox
{
    private readonly IReportDropbox _dropbox;
    private readonly ISeverityLookup _severityLookup;

    public SeverityAdjuster(
        IReportDropbox dropbox,
        ISeverityLookup severityLookup)
    {
        _dropbox = dropbox;
        _severityLookup = severityLookup;
    }

    public void Dropoff(ReportContextParameters parameters, IModGetter sourceMod, IMajorRecordGetter majorRecord, ITopic topic)
    {
        AdjustSeverity(topic);
        _dropbox.Dropoff(parameters, sourceMod, majorRecord, topic);
    }

    public void Dropoff(ReportContextParameters parameters, ITopic topic)
    {
        AdjustSeverity(topic);
        _dropbox.Dropoff(parameters, topic);
    }

    private void AdjustSeverity(ITopic topic)
    {
        var sev = _severityLookup.LookupSeverity(topic.TopicDefinition);
        topic.Severity = sev;
    }
}
