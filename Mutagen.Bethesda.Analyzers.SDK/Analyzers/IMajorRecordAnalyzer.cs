using JetBrains.Annotations;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.SDK.Analyzers
{
    [PublicAPI]
    public interface IMajorRecordAnalyzer<in TMajorRecordGetter> : IAnalyzer
        where TMajorRecordGetter : IMajorRecordGetter
    {
        MajorRecordAnalyzerResult? AnalyzeRecord(TMajorRecordGetter majorRecord);
    }
}
