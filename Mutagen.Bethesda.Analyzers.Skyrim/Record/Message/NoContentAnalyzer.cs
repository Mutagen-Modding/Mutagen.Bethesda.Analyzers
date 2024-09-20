using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
using Noggog;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Message;

public class NoContentAnalyzer : IIsolatedRecordAnalyzer<IMessageGetter>
{
    public static readonly TopicDefinition NoContent = MutagenTopicBuilder.DevelopmentTopic(
            "No Content",
            Severity.Suggestion)
        .WithoutFormatting("Message has no content");

    public IEnumerable<TopicDefinition> Topics { get; } = [NoContent];

    public RecordAnalyzerResult AnalyzeRecord(IsolatedRecordAnalyzerParams<IMessageGetter> param)
    {
        var message = param.Record;

        var result = new RecordAnalyzerResult();

        if ((message.Name is null || message.Name.String.IsNullOrWhitespace())
            && message.Description.String.IsNullOrWhitespace()
            && message.MenuButtons.Count == 0)
        {
            result.AddTopic(
                RecordTopic.Create(
                    message,
                    NoContent.Format(),
                    x => x));
        }

        return result;
    }
}
