using System.Collections;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Environments.DI;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Cache;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Reporting;

public sealed class TopicEnricher : IReportDropbox, IDisposable
{
    private readonly IReportDropbox _dropbox;
    private readonly ILinkCache _linkCache;

    public TopicEnricher(
        IReportDropbox dropbox,
        IGameEnvironmentProvider envGetter)
    {
        _dropbox = dropbox;
        _linkCache = envGetter.Construct().LinkCache;
    }

    public void Dropoff(IModGetter sourceMod, IMajorRecordGetter majorRecord, ITopic topic)
    {
        _dropbox.Dropoff(sourceMod, majorRecord, Enrich(topic));
    }

    public void Dropoff(ITopic topic)
    {
        _dropbox.Dropoff(Enrich(topic));
    }

    private FuncRecordTopic Enrich(ITopic topic)
    {
        object? ItemSelector(object? item) => item switch
        {
            string s => s,
            IFormLinkGetter link => _linkCache.TryResolve(link.FormKey, link.Type, out var record) ? record : item,
            IDictionary dictionary => dictionary.Keys.Cast<object>()
                .ToDictionary(key =>
                {
                    if (key is IFormLinkGetter keyLink)
                    {
                        return _linkCache.TryResolve(keyLink.FormKey, keyLink.Type, out var record) ? record : key;
                    }

                    return ItemSelector(key) ?? key;
                }, key =>
                {
                    if (dictionary[key] is IFormLinkGetter valueLink)
                    {
                        return _linkCache.TryResolve(valueLink.FormKey, valueLink.Type, out var record) ? record : dictionary[key];
                    }

                    return ItemSelector(dictionary[key]);
                }),
            IEnumerable enumerable => enumerable.Cast<object?>()
                .Select(e =>
                {
                    if (e is IFormLinkGetter link)
                    {
                        return _linkCache.TryResolve(link.FormKey, link.Type, out var record) ? record : e;
                    }

                    return ItemSelector(e);
                })
                .ToArray(),
            _ => item
        };

        return new FuncRecordTopic(topic, ItemSelector);
    }

    public void Dispose()
    {
        _linkCache.Dispose();
    }
}
