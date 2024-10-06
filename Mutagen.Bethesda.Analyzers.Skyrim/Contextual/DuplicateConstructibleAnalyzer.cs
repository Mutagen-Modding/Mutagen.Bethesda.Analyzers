using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
using Noggog;

namespace Mutagen.Bethesda.Analyzers.Skyrim.Contextual;

public class DuplicateConstructibleAnalyzer : IContextualAnalyzer
{
    private static readonly TopicDefinition<List<IConstructibleObjectGetter>> DuplicateConstructibleReference = MutagenTopicBuilder.DevelopmentTopic(
            "Duplicate Constructible Object",
            Severity.Warning)
        .WithFormatting<List<IConstructibleObjectGetter>>("Constructibles {0} are creating the same item, all but one should be removed.");

    public IEnumerable<TopicDefinition> Topics { get; } = [DuplicateConstructibleReference];

    private static readonly FuncEqualityComparer<IConstructibleObjectGetter> DuplicateConstructibleComparer = new((a, b) =>
    {
        if (a is null || b is null) return false;
        if (ReferenceEquals(a, b)) return true;

        return a.Conditions.Equals(b.Conditions) && Equals(a.Items, b.Items) && a.CreatedObject.Equals(b.CreatedObject) && a.WorkbenchKeyword.Equals(b.WorkbenchKeyword) && a.CreatedObjectCount == b.CreatedObjectCount;
    }, c => HashCode.Combine(c.Conditions, c.Items, c.CreatedObject, c.WorkbenchKeyword, c.CreatedObjectCount));

    public void Analyze(ContextualAnalyzerParams param)
    {
        var duplicateGroups = param.LinkCache.PriorityOrder.WinningOverrides<IConstructibleObjectGetter>()
            .GroupBy(x => x, DuplicateConstructibleComparer);

        foreach (var duplicateGroup in duplicateGroups)
        {
            if (duplicateGroup.Count() == 1) continue;

            param.AddTopic(
                DuplicateConstructibleReference.Format(duplicateGroup.ToList())
            );
        }
    }
}
