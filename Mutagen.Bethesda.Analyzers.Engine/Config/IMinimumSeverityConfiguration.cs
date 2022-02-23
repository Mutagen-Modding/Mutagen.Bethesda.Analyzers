using Mutagen.Bethesda.Analyzers.SDK.Topics;

namespace Mutagen.Bethesda.Analyzers.Config;

public interface IMinimumSeverityConfiguration
{
    Severity MinimumSeverity { get; }
}
