using System.Collections;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Reporting;

public class TopicListJoin : IReportDropbox
{
    private readonly IReportDropbox _dropbox;

    public TopicListJoin(IReportDropbox dropbox)
    {
        _dropbox = dropbox;
    }

    public void Dropoff(IModGetter sourceMod, IMajorRecordGetter majorRecord, ITopic topic)
    {
        _dropbox.Dropoff(sourceMod, majorRecord, SplitLists(topic));
    }

    public void Dropoff(ITopic topic)
    {
        _dropbox.Dropoff(SplitLists(topic));
    }

    private FuncRecordTopic SplitLists(ITopic topic)
    {
        object? ItemSelector(object? item) => item switch
        {
            string s => s,
            IDictionary dictionary => string.Join(", ", dictionary.Keys.Cast<object>().Select(key => $"[{key}: {ItemSelector(dictionary[key])}]").ToArray()),
            IEnumerable enumerable => string.Join(", ", enumerable.Cast<object?>().ToArray()),
            _ => item
        };

        return new FuncRecordTopic(topic, ItemSelector);
    }
}
