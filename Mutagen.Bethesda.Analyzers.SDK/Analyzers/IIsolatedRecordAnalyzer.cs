using JetBrains.Annotations;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.SDK.Analyzers
{
    [PublicAPI]
    public interface IIsolatedRecordAnalyzer<TMajor> : IAnalyzer
        where TMajor : IMajorRecordGetter
    {
        RecordAnalyzerResult? AnalyzeRecord(IsolatedRecordAnalyzerParams<TMajor> param);
    }
}
