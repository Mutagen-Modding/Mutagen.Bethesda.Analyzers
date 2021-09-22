using JetBrains.Annotations;

namespace Mutagen.Bethesda.Analyzers.SDK.Errors
{
    [PublicAPI]
    public record ErrorDefinition(
        string Id,
        string Title,
        string MessageFormat,
        Severity Severity);
}
