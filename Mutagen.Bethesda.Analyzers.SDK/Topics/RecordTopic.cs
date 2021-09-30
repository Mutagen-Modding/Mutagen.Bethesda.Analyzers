using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Mutagen.Bethesda.Analyzers.SDK.Topics
{
    [PublicAPI]
    public record RecordTopic(IFormattedTopicDefinition FormattedTopicDefinition, Expression MemberExpression) : ITopic
    {
        public static RecordTopic Create<T>(T obj, IFormattedTopicDefinition formattedTopicDefinition, Expression<Func<T, object>> memberExpression)
        {
            return new RecordTopic(formattedTopicDefinition, memberExpression);
        }
    }
}
