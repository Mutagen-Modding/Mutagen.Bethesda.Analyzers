using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
using Noggog;

namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.LeveledSpell;

public class CircularLeveledSpellListAnalyzer : IContextualRecordAnalyzer<ILeveledSpellGetter>
{
    public static readonly TopicDefinition<List<ILeveledSpellGetter>> CircularLeveledSpell = MutagenTopicBuilder.DevelopmentTopic(
            "Circular Leveled Spell",
            Severity.Suggestion)
        .WithFormatting<List<ILeveledSpellGetter>>("Leveled Spell contains itself in path {0}");

    public IEnumerable<TopicDefinition> Topics { get; } = [CircularLeveledSpell];

    public void AnalyzeRecord(ContextualRecordAnalyzerParams<ILeveledSpellGetter> param)
    {
        CircularLeveledListAnalyzerUtil.FindCircularList(param, l =>
        {
            if (l.Entries is not null)
            {
                return l.Entries
                    .Select(x => x.Data)
                    .NotNull()
                    .Select(x => x.Reference.FormKey);
            }

            return [];
        }, CircularLeveledSpell);
    }
    public IEnumerable<Func<ILeveledSpellGetter, object?>> FieldsOfInterest()
    {
        yield return x => x.Entries;
    }
}
