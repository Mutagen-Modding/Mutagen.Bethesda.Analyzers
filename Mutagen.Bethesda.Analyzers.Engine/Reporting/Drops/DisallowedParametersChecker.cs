using System.Collections;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Reporting.Drops;

public class DisallowedParametersChecker : IReportDropbox
{
    private readonly IReportDropbox _reportDropbox;

    public DisallowedParametersChecker(
        IReportDropbox reportDropbox)
    {
        _reportDropbox = reportDropbox;
    }

    public void Dropoff(
        ReportContextParameters parameters,
        IModGetter sourceMod,
        IMajorRecordGetter majorRecord,
        ITopic topic)
    {
        _reportDropbox.Dropoff(parameters, sourceMod, majorRecord, Check(parameters, topic));
    }

    public void Dropoff(
        ReportContextParameters parameters,
        ITopic topic)
    {
        _reportDropbox.Dropoff(parameters, Check(parameters, topic));
    }

    private ITopic Check(
        ReportContextParameters parameters,
        ITopic topic)
    {
        return topic.WithFormattedTopic(
            topic.FormattedTopic.Transform(parameters, Checker));
    }

    private object? Checker(
        ReportContextParameters parameters,
        object? item)
    {
        if (item is IEnumerable)
        {
            throw new ArgumentException("Enumerables are not allowed in formatted topic parameters");
        }
        if (item is FormKey)
        {
            throw new ArgumentException("FormKeys are not allowed in formatted topic parameters.  Pass in typed FormLinks instead");
        }
        return item;
    }
}
