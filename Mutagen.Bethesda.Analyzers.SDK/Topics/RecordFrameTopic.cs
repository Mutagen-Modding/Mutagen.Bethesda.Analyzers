namespace Mutagen.Bethesda.Analyzers.SDK.Topics;

public record RecordFrameTopic(IFormattedTopicDefinition _formattedTopicDefinition) : ITopic
{
    private readonly IFormattedTopicDefinition _formattedTopicDefinition = _formattedTopicDefinition;

    public static RecordFrameTopic Create(IFormattedTopicDefinition formattedTopicDefinition)
    {
        return new RecordFrameTopic(formattedTopicDefinition);
    }

    public TopicDefinition TopicDefinition => _formattedTopicDefinition.TopicDefinition;
    public string FormattedMessage => string.Format(TopicDefinition.MessageFormat, Items);
    public Severity Severity { get; set; } = _formattedTopicDefinition.TopicDefinition.Severity;
    public IEnumerable<object?> Items => _formattedTopicDefinition.Items;
}
