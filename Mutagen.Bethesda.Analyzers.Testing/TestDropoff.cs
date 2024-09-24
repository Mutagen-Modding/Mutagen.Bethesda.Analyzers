using Mutagen.Bethesda.Analyzers.Reporting;
using Mutagen.Bethesda.Analyzers.Reporting.Drops;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Testing;

public class TestDropoff : IReportDropbox
{
    public int TotalReports => Reports.Count;
    private readonly List<ITopic> _reports = new();
    public IReadOnlyList<ITopic> Reports => _reports;

    public void Dropoff(
        ReportContextParameters parameters,
        IModGetter sourceMod,
        IMajorRecordGetter majorRecord,
        ITopic topic)
    {
        _reports.Add(topic);
    }

    public void Dropoff(
        ReportContextParameters parameters,
        ITopic topic)
    {
        _reports.Add(topic);
    }
}
