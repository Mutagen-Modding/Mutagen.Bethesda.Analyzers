namespace Mutagen.Bethesda.Analyzers.SDK.Topics;

public interface IFormattedTopicDefinition
{
    TopicDefinition TopicDefinition { get; }
    string FormattedMessage { get; }
}

public class FormattedTopicDefinition : IFormattedTopicDefinition
{
    public TopicDefinition TopicDefinition { get; }

    public FormattedTopicDefinition(TopicDefinition topicDefinition, string message)
    {
        TopicDefinition = topicDefinition;
    }

    public override string ToString() => FormattedMessage;

    public string FormattedMessage => TopicDefinition.MessageFormat;
}

public class FormattedTopicDefinition<T1> : IFormattedTopicDefinition
{
    public TopicDefinition TopicDefinition { get; }
    public T1 Item1 { get; }

    public FormattedTopicDefinition(
        TopicDefinition topicDefinition,
        T1 item1)
    {
        TopicDefinition = topicDefinition;
        Item1 = item1;
    }

    public override string ToString() => FormattedMessage;

    public string FormattedMessage => string.Format(TopicDefinition.MessageFormat, Item1);
}

public class FormattedTopicDefinition<T1, T2> : IFormattedTopicDefinition
{
    public TopicDefinition TopicDefinition { get; }
    public T1 Item1 { get; }
    public T2 Item2 { get; }

    public FormattedTopicDefinition(
        TopicDefinition topicDefinition,
        T1 item1,
        T2 item2)
    {
        TopicDefinition = topicDefinition;
        Item1 = item1;
        Item2 = item2;
    }

    public override string ToString() => FormattedMessage;

    public string FormattedMessage => string.Format(TopicDefinition.MessageFormat, Item1, Item2);
}

public class FormattedTopicDefinition<T1, T2, T3> : IFormattedTopicDefinition
{
    public TopicDefinition TopicDefinition { get; }
    public T1 Item1 { get; }
    public T2 Item2 { get; }
    public T3 Item3 { get; }

    public FormattedTopicDefinition(
        TopicDefinition topicDefinition,
        T1 item1,
        T2 item2,
        T3 item3)
    {
        TopicDefinition = topicDefinition;
        Item1 = item1;
        Item2 = item2;
        Item3 = item3;
    }

    public override string ToString() => FormattedMessage;

    public string FormattedMessage => string.Format(TopicDefinition.MessageFormat, Item1, Item2, Item3);
}

public class FormattedTopicDefinition<T1, T2, T3, T4> : IFormattedTopicDefinition
{
    public TopicDefinition TopicDefinition { get; }
    public T1 Item1 { get; }
    public T2 Item2 { get; }
    public T3 Item3 { get; }
    public T4 Item4 { get; }

    public FormattedTopicDefinition(
        TopicDefinition topicDefinition,
        T1 item1,
        T2 item2,
        T3 item3,
        T4 item4)
    {
        TopicDefinition = topicDefinition;
        Item1 = item1;
        Item2 = item2;
        Item3 = item3;
        Item4 = item4;
    }

    public override string ToString() => FormattedMessage;

    public string FormattedMessage => string.Format(TopicDefinition.MessageFormat, Item1, Item2, Item3, Item4);
}
