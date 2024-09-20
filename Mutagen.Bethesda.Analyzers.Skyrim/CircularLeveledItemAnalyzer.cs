using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Noggog;
namespace Mutagen.Bethesda.Analyzers.Skyrim;

public partial class CircularLeveledListAnalyzer : IContextualRecordAnalyzer<ILeveledItemGetter>
{
    public static readonly TopicDefinition<string> CircularLeveledItem = MutagenTopicBuilder.DevelopmentTopic(
            "Circular Leveled Item",
            Severity.Suggestion)
        .WithFormatting<string>("Leveled Item contains itself in path {0}");

    public RecordAnalyzerResult AnalyzeRecord(ContextualRecordAnalyzerParams<ILeveledItemGetter> param)
    {
        return FindCircularList(param.Record, l =>
        {
            if (l.Entries is not null)
            {
                return l.Entries
                    .Select(x => x.Data)
                    .NotNull()
                    .Select(x => x.Reference.FormKey);
            }

            return [];
        }, param.LinkCache, CircularLeveledItem);
    }
}
