using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Spell;

public class NoMenuDisplayObjectAnalyzer : IContextualRecordAnalyzer<ISpellGetter>
{
    public static readonly TopicDefinition NoMenuDisplayObject = MutagenTopicBuilder.DevelopmentTopic(
            "No Menu Display Object",
            Severity.Suggestion)
        .WithoutFormatting("Spell has no menu display object and none of its effects have it - it will be invisible in the magic menu");

    public IEnumerable<TopicDefinition> Topics { get; } = [NoMenuDisplayObject];

    public void AnalyzeRecord(ContextualRecordAnalyzerParams<ISpellGetter> param)
    {
        var spell = param.Record;

        if (!spell.MenuDisplayObject.IsNull) return;

        foreach (var effect in spell.Effects)
        {
            var magicEffect = effect.BaseEffect.TryResolve(param.LinkCache);
            if (magicEffect is null) continue;

            if (!magicEffect.MenuDisplayObject.IsNull)
            {
                return;
            }
        }

        param.AddTopic(
            NoMenuDisplayObject.Format(),
            x => x);
    }
}
