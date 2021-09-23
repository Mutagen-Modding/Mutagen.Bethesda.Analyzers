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
            return $"[{SeverityString(Severity)}] [{Id}] {Title}: {MessageFormat}";
        }

        private static string SeverityString(Severity sev)
        {
            return sev switch
            {
                Severity.Suggestion => "SUG",
                Severity.Warning => "WAR",
                Severity.Error => "ERR",
                Severity.CTD => "CTD",
                _ => throw new ArgumentOutOfRangeException(nameof(sev), sev, null)
            };
        }
    }
}
