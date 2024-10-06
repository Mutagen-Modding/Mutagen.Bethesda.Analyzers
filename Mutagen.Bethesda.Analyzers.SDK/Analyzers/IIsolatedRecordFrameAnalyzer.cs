using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.SDK.Analyzers;

public interface IIsolatedRecordFrameAnalyzer<TMajor> : IAnalyzer
    where TMajor : IMajorRecordGetter
{
    void AnalyzeRecord(IsolatedRecordFrameAnalyzerParams<TMajor> param);
}

public interface IIsolatedRecordFrameAnalyzer : IAnalyzer
{
    void AnalyzeRecord(IsolatedRecordFrameAnalyzerParams param);
}
