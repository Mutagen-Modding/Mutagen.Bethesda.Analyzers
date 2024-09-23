using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
using Noggog;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Quest;

public class DialogueAliasAnalyzer : IContextualRecordAnalyzer<IQuestGetter>
{
    public static readonly TopicDefinition<string?, List<IDialogResponsesGetter>> InvalidDialogueAlias = MutagenTopicBuilder.DevelopmentTopic(
            "Invalid Dialogue Alias",
            Severity.Warning)
        .WithFormatting<string?, List<IDialogResponsesGetter>>("Alias {0} that is forced to none, has no additional dialogue formlist/npc and has dialogue conditioned to it {1} doesn't get any lines for VA exported");

    public IEnumerable<TopicDefinition> Topics { get; } = [InvalidDialogueAlias];

    public RecordAnalyzerResult? AnalyzeRecord(ContextualRecordAnalyzerParams<IQuestGetter> param)
    {
        var quest = param.Record;
        var relevantAliases = quest.Aliases
            .Where(alias => alias.ForcedReference.IsNull
                            && !alias.Conditions.Any()
                            && alias.SpecificLocation.IsNull
                            && alias.UniqueActor.IsNull
                            && alias.VoiceTypes.IsNull
                            && alias.External is null
                            && alias.CreateReferenceToObject is null
                            && alias.FindMatchingRefFromEvent is null
                            && alias.FindMatchingRefNearAlias is null)
            .ToList();

        if (relevantAliases.Count == 0) return null;

        var referencedDialogue = new Dictionary<IQuestAliasGetter, List<IDialogResponsesGetter>>();

        // TODO: potentially replace with reference cache
        foreach (var topic in param.LinkCache.PriorityOrder.WinningOverrides<IDialogTopicGetter>())
        {
            if (quest.FormKey != topic.Quest.FormKey) continue;

            foreach (var response in topic.Responses)
            {
                if (response.Speaker.IsNull) continue;

                foreach (var condition in response.Conditions)
                {
                    if (condition.Data is not IGetIsAliasRefConditionDataGetter { ReferenceAliasIndex: var aliasIndex }) continue;
                    if (condition is IConditionFloatGetter { ComparisonValue: 0 }) continue;

                    var alias = relevantAliases.Find(x => x.ID == aliasIndex);
                    if (alias is null) continue;

                    referencedDialogue.GetOrAdd(alias).Add(response);
                }
            }
        }

        var result = new RecordAnalyzerResult();

        foreach (var (alias, dialogue) in referencedDialogue) {
            if (dialogue.Count == 0) continue;

            result.AddTopic(
                RecordTopic.Create(
                    alias,
                    InvalidDialogueAlias.Format(alias.Name, dialogue),
                    x => x.Conditions));
        }

        return result;
    }
}
