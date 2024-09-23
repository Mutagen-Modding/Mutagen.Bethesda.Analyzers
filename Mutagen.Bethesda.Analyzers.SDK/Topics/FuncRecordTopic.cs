namespace Mutagen.Bethesda.Analyzers.SDK.Topics;

public sealed class FuncRecordTopic : ITopic
{
    private readonly ITopic _topic;
    private readonly Func<object?, object?> _itemSelector;

    public FuncRecordTopic(ITopic topic, Func<object?, object?> itemSelector)
    {
        _topic = topic;
        _itemSelector = itemSelector;
    }

    public TopicDefinition TopicDefinition => _topic.TopicDefinition;
    public string FormattedMessage => string.Format(TopicDefinition.MessageFormat, Items.ToArray());
    public Severity Severity
    {
        get => _topic.Severity;
        set => _topic.Severity = value;
    }
    public IEnumerable<object?> Items => _topic.Items.Select(_itemSelector);
}
