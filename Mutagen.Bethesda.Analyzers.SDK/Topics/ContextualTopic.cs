namespace Mutagen.Bethesda.Analyzers.SDK.Topics;

public record ContextualTopic(IFormattedTopicDefinition _formattedTopicDefinition) : ITopic
{
    private readonly IFormattedTopicDefinition _formattedTopicDefinition = _formattedTopicDefinition;

    public static ContextualTopic Create<T>(T obj, IFormattedTopicDefinition formattedTopicDefinition)
    {
        return new ContextualTopic(formattedTopicDefinition);
    }

    public TopicDefinition TopicDefinition => _formattedTopicDefinition.TopicDefinition;
    public string FormattedMessage => string.Format(TopicDefinition.MessageFormat, Items.ToArray());
    public Severity Severity { get; set; } = _formattedTopicDefinition.TopicDefinition.Severity;
    public IEnumerable<object?> Items => _formattedTopicDefinition.Items;
}
