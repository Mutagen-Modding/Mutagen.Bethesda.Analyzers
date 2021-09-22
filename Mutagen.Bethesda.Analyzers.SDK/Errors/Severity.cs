using JetBrains.Annotations;

namespace Mutagen.Bethesda.Analyzers.SDK.Errors
{
    [PublicAPI]
    public enum Severity : byte
    {
        Suggestion = 0,
        Warning = 1,
        Error = 2,
        CTD = 3
    }
}
