using System;
using JetBrains.Annotations;

namespace Mutagen.Bethesda.Analyzers.SDK.Errors
{
    [PublicAPI]
    public record ErrorDefinition(
        string Id,
        string Title,
        string MessageFormat,
        Severity Severity)
    {
        public override string ToString()
        {
            return $"[{Severity.ToShortString()}] [{Id}] {Title}: {MessageFormat}";
        }

        public FormattedErrorDefinition Format(params object?[]? formatArgs)
        {
            return new FormattedErrorDefinition(
                this,
                formatArgs == null
                    ? MessageFormat
                    : string.Format(MessageFormat, formatArgs));
        }
    }
}
