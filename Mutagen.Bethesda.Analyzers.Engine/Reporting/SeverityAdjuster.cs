using Mutagen.Bethesda.Analyzers.Config;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Reporting;

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

    public void Dropoff(IModGetter sourceMod, IMajorRecordGetter majorRecord, ITopic topic)
    {
        AdjustSeverity(topic);
        _dropbox.Dropoff(sourceMod, majorRecord, topic);
    }

    public void Dropoff(ITopic topic)
    {
        AdjustSeverity(topic);
        _dropbox.Dropoff(topic);
    }

    private void AdjustSeverity(ITopic topic)
    {
        var sev = _severityLookup.LookupSeverity(topic.TopicDefinition);
        topic.Severity = sev;
    }
}
