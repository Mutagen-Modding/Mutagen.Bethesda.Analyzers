namespace Mutagen.Bethesda.Analyzers.SDK.Topics;

public record Topic
{
    public static Topic Create(
        IFormattedTopicDefinition formattedTopicDefinition)
    {
        return new Topic
        {
            FormattedTopic = formattedTopicDefinition,
            Severity = formattedTopicDefinition.TopicDefinition.Severity
        };
    }

    public TopicDefinition TopicDefinition => FormattedTopic.TopicDefinition;
    public required IFormattedTopicDefinition FormattedTopic { get; init; }
    public required Severity Severity { get; init; }
}
