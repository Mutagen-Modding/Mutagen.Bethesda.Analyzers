using Mutagen.Bethesda.Analyzers.SDK.Drops;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Testing;

public class TestDropoff : IReportDropbox
{
    private readonly List<ITopic> _reports = new();
    public IReadOnlyList<ITopic> Reports => _reports;

    public void Dropoff(ReportContextParameters parameters, ModKey mod, IMajorRecordIdentifier record, ITopic topic)
    {
        _reports.Add(topic);
    }

    public void Dropoff(ReportContextParameters parameters, ITopic topic)
    {
        _reports.Add(topic);
    }
}
