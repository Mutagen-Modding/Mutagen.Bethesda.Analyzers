using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Noggog;
namespace Mutagen.Bethesda.Analyzers.Skyrim;

public partial class CircularLeveledListAnalyzer : IContextualRecordAnalyzer<ILeveledSpellGetter>
{
    public static readonly TopicDefinition<List<ILeveledSpellGetter>> CircularLeveledSpell = MutagenTopicBuilder.DevelopmentTopic(
            "Circular Leveled Spell",
            Severity.Suggestion)
        .WithFormatting<List<ILeveledSpellGetter>>("Leveled Spell contains itself in path {0}");

    public RecordAnalyzerResult AnalyzeRecord(ContextualRecordAnalyzerParams<ILeveledSpellGetter> param)
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
        }, param.LinkCache, CircularLeveledSpell);
    }
}
