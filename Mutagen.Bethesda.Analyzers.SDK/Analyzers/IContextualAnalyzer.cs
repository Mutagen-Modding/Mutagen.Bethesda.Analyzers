namespace Mutagen.Bethesda.Analyzers.SDK.Analyzers;

public interface IContextualAnalyzer : IAnalyzer
{
    void Analyze(ContextualAnalyzerParams param);
}
