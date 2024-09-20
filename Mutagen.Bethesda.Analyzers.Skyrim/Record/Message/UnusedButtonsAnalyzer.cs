using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
using Noggog;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Message;

public class UnusedButtonsAnalyzer : IIsolatedRecordAnalyzer<IMessageGetter>
{
    public static readonly TopicDefinition<int> UnusedButtons = MutagenTopicBuilder.DevelopmentTopic(
            "Unused Buttons",
            Severity.Suggestion)
        .WithFormatting<int>("Notification message has {0} buttons that will not be displayed");

    public IEnumerable<TopicDefinition> Topics { get; } = [UnusedButtons];

    public RecordAnalyzerResult? AnalyzeRecord(IsolatedRecordAnalyzerParams<IMessageGetter> param)
    {
        var message = param.Record;

        if (message.Flags.HasFlag(Bethesda.Skyrim.Message.Flag.MessageBox)) return null;
        if (message.MenuButtons.Count == 0) return null;

        return new RecordAnalyzerResult(
            RecordTopic.Create(
                message,
                UnusedButtons.Format(message.MenuButtons.Count),
                x => x.MenuButtons));
    }
}
