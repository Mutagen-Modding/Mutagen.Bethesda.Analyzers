namespace Mutagen.Bethesda.Analyzers.SDK.Topics;

public interface ITopic
{
    TopicDefinition TopicDefinition { get; }
    IFormattedTopicDefinition FormattedTopic { get; }
    Severity Severity { get; }
    ITopic WithFormattedTopic(IFormattedTopicDefinition formattedTopicDefinition);
    ITopic WithSeverity(Severity severity);
}
