using System.Linq.Expressions;

namespace Mutagen.Bethesda.Analyzers.SDK.Topics;

public record RecordTopic : ITopic
{
    public static RecordTopic Create<T>(
        T obj,
        IFormattedTopicDefinition formattedTopicDefinition,
        Expression<Func<T, object?>> memberExpression)
    {
        return new RecordTopic
        {
            MemberExpression = memberExpression,
            FormattedTopic = formattedTopicDefinition,
            Severity = formattedTopicDefinition.TopicDefinition.Severity
        };
    }

    public TopicDefinition TopicDefinition => FormattedTopic.TopicDefinition;
    public required IFormattedTopicDefinition FormattedTopic { get; init; }
    public required Severity Severity { get; init; }
    public required Expression MemberExpression { get; init; }
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
