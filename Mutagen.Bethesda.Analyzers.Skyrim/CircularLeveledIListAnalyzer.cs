using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Cache;
using Mutagen.Bethesda.Plugins.Records;
namespace Mutagen.Bethesda.Analyzers.Skyrim;

public partial class CircularLeveledListAnalyzer
{
    public IEnumerable<TopicDefinition> Topics => [CircularLeveledItem, CircularLeveledNpc, CircularLeveledSpell];

    private static RecordAnalyzerResult FindCircularList<T>(T leveled, Func<T, IEnumerable<FormKey>> nestedEntriesSelector, ILinkCache linkCache, TopicDefinition<string> topic)
        where T : class, IMajorRecordGetter
    {
        var report = new RecordAnalyzerResult();
        var stack = new Stack<T>();

        FindCircularListInternal(leveled);

        return report;

        void FindCircularListInternal(T t)
        {
            if (stack.Any(x => x.FormKey == t.FormKey))
            {
                report.AddTopic(
                    RecordTopic.Create(
                        t,
                        topic.Format(string.Join(" -> ", stack.Select(x => x.EditorID))),
                        x => x
                    )
                );
                return;
            }

            stack.Push(t);

            foreach (var formKey in nestedEntriesSelector(t))
            {
                if (!linkCache.TryResolve<T>(formKey, out var leveledList)) continue;

                FindCircularListInternal(leveledList);
            }

            stack.Pop();
        }
    }
}
