using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
using Noggog;

namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Quest;

public class StoryManagerQuestAnalyzer : IContextualRecordAnalyzer<IQuestGetter>
{
    public static readonly TopicDefinition StoryManagerQuestNotAssigned = MutagenTopicBuilder.DevelopmentTopic(
            "Story Manager Quest not assigned",
            Severity.Warning)
        .WithoutFormatting("Quest with Story Manager Event not assigned to any Story Manager Quest Node");

    public IEnumerable<TopicDefinition> Topics { get; } = [StoryManagerQuestNotAssigned];

    public void AnalyzeRecord(ContextualRecordAnalyzerParams<IQuestGetter> param)
    {
        var quest = param.Record;
        if (!quest.Event.HasValue) return;

        // TODO: potentially replace with reference cache

        foreach (var mod in param.LoadOrder.Select(l => l.Value.Mod).NotNull())
        {
            foreach (var questNode in mod.EnumerateMajorRecords<IStoryManagerQuestNodeGetter>())
            {
                foreach (var questFormKey in questNode.Quests.Select(n => n.Quest.FormKey))
                {
                    if (questFormKey == quest.FormKey)
                    {
                        param.AddTopic(
                            StoryManagerQuestNotAssigned.Format());
                        return;
                    }
                }
            }
        }
    }

    public IEnumerable<Func<IQuestGetter, object?>> FieldsOfInterest()
    {
        yield return x => x.Event;
    }
}
