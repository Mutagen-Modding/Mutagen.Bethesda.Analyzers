using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
using Noggog;

namespace Mutagen.Bethesda.Analyzers.Skyrim;

public partial class CircularLeveledListAnalyzer : IContextualRecordAnalyzer<ILeveledItemGetter>
{
    public static readonly TopicDefinition<List<ILeveledItemGetter>> CircularLeveledItem = MutagenTopicBuilder.DevelopmentTopic(
            "Circular Leveled Item",
            Severity.Suggestion)
        .WithFormatting<List<ILeveledItemGetter>>("Leveled Item contains itself in path {0}");

    public void AnalyzeRecord(ContextualRecordAnalyzerParams<ILeveledItemGetter> param)
    {
        FindCircularList(param.Record, l =>
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
