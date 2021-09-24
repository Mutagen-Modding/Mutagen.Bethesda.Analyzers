using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Mutagen.Bethesda.Analyzers.SDK.Errors
{
    [PublicAPI]
    public record RecordError(FormattedErrorDefinition FormattedErrorDefinition, Expression MemberExpression) : IError
    {
        public static RecordError Create<T>(T obj, FormattedErrorDefinition formattedErrorDefinition, Expression<Func<T, object>> memberExpression)
        {
            return new RecordError(formattedErrorDefinition, memberExpression);
        }
    }
}
