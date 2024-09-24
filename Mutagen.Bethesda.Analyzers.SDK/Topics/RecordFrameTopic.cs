namespace Mutagen.Bethesda.Analyzers.SDK.Topics;

public record RecordFrameTopic : ITopic
{
    public static RecordFrameTopic Create(IFormattedTopicDefinition formattedTopicDefinition)
    {
        return new RecordFrameTopic
        {
            FormattedTopic = formattedTopicDefinition,
            Severity = formattedTopicDefinition.TopicDefinition.Severity
        };
    }

    public TopicDefinition TopicDefinition => FormattedTopic.TopicDefinition;
    public required IFormattedTopicDefinition FormattedTopic { get; init; }
    public required Severity Severity { get; init; }
    public ITopic WithFormattedTopic(IFormattedTopicDefinition formattedTopicDefinition)
    {
        return this with
        {
            FormattedTopic = formattedTopicDefinition
        };
    }
    public ITopic WithSeverity(Severity severity)
    {
        return this with
        {
            Severity = severity
        };
    }
}
