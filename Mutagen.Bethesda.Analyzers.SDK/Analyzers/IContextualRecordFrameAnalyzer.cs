using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.SDK.Analyzers;

public interface IContextualRecordFrameAnalyzer<TMajor> : IAnalyzer
    where TMajor : IMajorRecordGetter
{
    RecordAnalyzerResult? AnalyzeRecord(ContextualRecordFrameAnalyzerParams<TMajor> param);
}

public interface IContextualRecordFrameAnalyzer : IAnalyzer
{
    bool AcceptsType<TMajor>() where TMajor : IMajorRecordGetter;

    RecordAnalyzerResult? AnalyzeRecord(ContextualRecordFrameAnalyzerParams param);
}
