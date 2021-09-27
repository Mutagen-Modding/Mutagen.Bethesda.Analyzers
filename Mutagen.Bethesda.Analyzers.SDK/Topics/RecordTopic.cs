using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Mutagen.Bethesda.Analyzers.SDK.Topics
{
    [PublicAPI]
    public record RecordTopic(FormattedTopicDefinition FormattedTopicDefinition, Expression MemberExpression) : ITopic
    {
        public static RecordTopic Create<T>(T obj, FormattedTopicDefinition formattedTopicDefinition, Expression<Func<T, object>> memberExpression)
        {
            return new RecordTopic(formattedTopicDefinition, memberExpression);
        }
    }
}
