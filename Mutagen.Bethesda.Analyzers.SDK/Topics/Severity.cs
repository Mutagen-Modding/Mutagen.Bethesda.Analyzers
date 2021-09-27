using System;
using JetBrains.Annotations;

namespace Mutagen.Bethesda.Analyzers.SDK.Topics
{
    [PublicAPI]
    public enum Severity : byte
    {
        Suggestion = 0,
        Warning = 1,
        Error = 2,
        CTD = 3
    }

    public static class SeverityExt
    {
        public static string ToShortString(this Severity sev)
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
