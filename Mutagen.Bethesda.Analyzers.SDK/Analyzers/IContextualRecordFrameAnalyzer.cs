using JetBrains.Annotations;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.SDK.Analyzers
{
    [PublicAPI]
    public interface IContextualRecordFrameAnalyzer<TMajor> : IAnalyzer
        where TMajor : IMajorRecordGetter
    {
        RecordAnalyzerResult? AnalyzeRecord(ContextualRecordFrameAnalyzerParams<TMajor> param);
    }

    [PublicAPI]
    public interface IContextualRecordFrameAnalyzer : IAnalyzer
    {
        bool AcceptsType<TMajor>() where TMajor : IMajorRecordGetter;

        RecordAnalyzerResult? AnalyzeRecord(ContextualRecordFrameAnalyzerParams param);
    }
}
