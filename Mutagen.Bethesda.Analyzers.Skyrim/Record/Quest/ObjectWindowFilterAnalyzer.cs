using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
using Noggog;

namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Quest;

public class ObjectWindowFilterAnalyzer : IIsolatedRecordAnalyzer<IQuestGetter>
{
    public static readonly TopicDefinition NoObjectWindowFilter = MutagenTopicBuilder.DevelopmentTopic(
            "No Object Window Filter",
            Severity.Suggestion)
        .WithoutFormatting("Quest has no Object Window Filter");

    public IEnumerable<TopicDefinition> Topics { get; } = [NoObjectWindowFilter];

    public void AnalyzeRecord(IsolatedRecordAnalyzerParams<IQuestGetter> param)
    {
        var quest = param.Record;
        if (quest.Filter.IsNullOrWhitespace())
        {
            param.AddTopic(
                NoObjectWindowFilter.Format(),
                x => x.Filter);
        }
    }
}
