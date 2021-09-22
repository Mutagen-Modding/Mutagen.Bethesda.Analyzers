using JetBrains.Annotations;

namespace Mutagen.Bethesda.Analyzers.SDK.Errors
{
    [PublicAPI]
    public interface IError
    {
        ErrorDefinition ErrorDefinition { get; }
    }
}
