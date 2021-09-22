using JetBrains.Annotations;
using Mutagen.Bethesda.Analyzers.SDK.Result;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.SDK.Analyzers
{
    [PublicAPI]
    public interface IMajorRecordAnalyzer<in TMajorRecordGetter> : IAnalyzer
        where TMajorRecordGetter : IMajorRecordGetter
    {
        AnalyzerResult? AnalyzeRecord(TMajorRecordGetter majorRecord);
    }
}
