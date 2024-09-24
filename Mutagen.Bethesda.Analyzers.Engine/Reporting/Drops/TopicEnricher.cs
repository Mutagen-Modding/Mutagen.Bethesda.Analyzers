using System.Collections;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Reporting.Drops;

public sealed class TopicEnricher : IReportDropbox
{
    private readonly IReportDropbox _reportDropbox;
    public TopicEnricher(
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
        _reportDropbox.Dropoff(parameters, sourceMod, majorRecord, Enrich(parameters, topic));
    }

    public void Dropoff(
        ReportContextParameters parameters,
        ITopic topic)
    {
        _reportDropbox.Dropoff(parameters, Enrich(parameters, topic));
    }

    private FuncRecordTopic Enrich(
        ReportContextParameters parameters,
        ITopic topic)
    {
        object? ItemSelector(object? item) => item switch
        {
            string s => s,
            IFormLinkGetter link => parameters.LinkCache.TryResolve(link.FormKey, link.Type, out var record) ? record : item,
            IDictionary dictionary => dictionary.Keys.Cast<object>()
                .ToDictionary(key =>
                {
                    if (key is IFormLinkGetter keyLink)
                    {
                        return parameters.LinkCache.TryResolve(keyLink.FormKey, keyLink.Type, out var record) ? record : key;
                    }

                    return ItemSelector(key) ?? key;
                }, key =>
                {
                    if (dictionary[key] is IFormLinkGetter valueLink)
                    {
                        return parameters.LinkCache.TryResolve(valueLink.FormKey, valueLink.Type, out var record) ? record : dictionary[key];
                    }

                    return ItemSelector(dictionary[key]);
                }),
            IEnumerable enumerable => enumerable.Cast<object?>()
                .Select(e =>
                {
                    if (e is IFormLinkGetter link)
                    {
                        return parameters.LinkCache.TryResolve(link.FormKey, link.Type, out var record) ? record : e;
                    }

                    return ItemSelector(e);
                })
                .ToArray(),
            _ => item
        };

        return new FuncRecordTopic(topic, ItemSelector);
    }
}
