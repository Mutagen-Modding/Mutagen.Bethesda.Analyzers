using Mutagen.Bethesda.Analyzers.SDK.Results;

namespace Mutagen.Bethesda.Analyzers.SDK.Analyzers;

public interface IContextualAnalyzer : IAnalyzer
{
    ContextualAnalyzerResult? Analyze(ContextualAnalyzerParams param);
}
