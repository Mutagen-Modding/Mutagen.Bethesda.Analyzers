using System.Collections;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Reporting.Drops;

public class TopicListJoin : IReportDropbox
{
    private readonly IReportDropbox _reportDropbox;
    public TopicListJoin(IReportDropbox reportDropbox)
    {
        _reportDropbox = reportDropbox;
    }

    public void Dropoff(
        ReportContextParameters parameters,
        IModGetter sourceMod,
        IMajorRecordGetter majorRecord,
        ITopic topic)
    {
        _reportDropbox.Dropoff(parameters, sourceMod, majorRecord, SplitLists(topic));
    }

    public void Dropoff(
        ReportContextParameters parameters,
        ITopic topic)
    {
        _reportDropbox.Dropoff(parameters, SplitLists(topic));
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
