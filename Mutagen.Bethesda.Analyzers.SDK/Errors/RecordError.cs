using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.SDK.Errors
{
    [PublicAPI]
    public record RecordError(
        ErrorDefinition ErrorDefinition,
        RecordType RecordType,
        FormKey FormKey,
        Expression MemberExpression) : IError
    {
        public static RecordError Create<TMajorRecordGetter>(
            ErrorDefinition errorDefinition,
            TMajorRecordGetter majorRecord,
            RecordType recordType,
            Expression<Func<TMajorRecordGetter, object>> memberExpression)
            where TMajorRecordGetter : IMajorRecordGetter
        {
            return new RecordError(errorDefinition, recordType, majorRecord.FormKey, memberExpression);
        }
    }
}
