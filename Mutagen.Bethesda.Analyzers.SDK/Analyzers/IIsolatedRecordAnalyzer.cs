using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.SDK.Analyzers;

public interface IIsolatedRecordAnalyzer<TMajor> : IAnalyzer
    where TMajor : IMajorRecordGetter
{
    void AnalyzeRecord(IsolatedRecordAnalyzerParams<TMajor> param);
}
