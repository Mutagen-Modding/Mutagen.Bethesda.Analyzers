using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Dialog.Responses;

public class InvalidSayOnceAnalyzer : IContextualRecordAnalyzer<IDialogResponsesGetter>
{
    public static readonly TopicDefinition<string> InvalidSayOnce = MutagenTopicBuilder.DevelopmentTopic(
            "Invalid SayOnce",
            Severity.Error)
        .WithFormatting<string>("Dialog is say once although it's quest {0} is not Start Game Enabled");

    public IEnumerable<TopicDefinition> Topics { get; } = [InvalidSayOnce];

    public RecordAnalyzerResult? AnalyzeRecord(ContextualRecordAnalyzerParams<IDialogResponsesGetter> param)
    {
        var dialogResponses = param.Record;

        if (dialogResponses.Flags is null || (dialogResponses.Flags.Flags & DialogResponses.Flag.SayOnce) == 0) return null;

        var context = param.LinkCache.ResolveSimpleContext<IDialogResponsesGetter>(dialogResponses.FormKey);
        if (context.Parent?.Record is not IDialogTopicGetter topic) return null;

        var quest = topic.Quest.TryResolve(param.LinkCache);
        if (quest is null || quest.Flags.HasFlag(Quest.Flag.StartGameEnabled))
        {
            return null;
        }

        return new RecordAnalyzerResult(
            RecordTopic.Create(
                dialogResponses,
                InvalidSayOnce.Format(quest.EditorID ?? quest.FormKey.ToString()),
                x => x.Flags
            )
        );
    }
}
