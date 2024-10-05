using Mutagen.Bethesda.Analyzers.SDK.Drops;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Reporting.Drops;

public sealed class EditorIdEnricher : IReportDropbox
{
    private readonly IReportDropbox _reportDropbox;

    public EditorIdEnricher(
        IReportDropbox reportDropbox)
    {
        _reportDropbox = reportDropbox;
    }

    public void Dropoff(
        ReportContextParameters parameters,
        ModKey mod,
        IMajorRecordIdentifier record,
        ITopic topic)
    {
        _reportDropbox.Dropoff(parameters, mod, record, Enrich(parameters, topic));
    }

    public void Dropoff(
        ReportContextParameters parameters,
        ITopic topic)
    {
        _reportDropbox.Dropoff(parameters, Enrich(parameters, topic));
    }

    private ITopic Enrich(
        ReportContextParameters parameters,
        ITopic topic)
    {
        return topic.WithFormattedTopic(topic.FormattedTopic.Transform(parameters, Selector));
    }

    private object LinkResolver(
        ReportContextParameters parameters,
        IFormLinkGetter link)
    {
        if (parameters.LinkCache.TryResolveIdentifier(link.FormKey, link.Type, out var edid)
            && edid != null)
        {
            return new MajorRecordIdentifier()
            {
                FormKey = link.FormKey,
                EditorID = edid
            };
        }

        return link;
    }

    private object? Selector(
        ReportContextParameters parameters,
        object? item)
    {
        if (item is IFormLinkGetter link)
        {
            return LinkResolver(parameters, link);
        }
        return item;
    }
}
