using JetBrains.Annotations;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.SDK.Analyzers
{
    [PublicAPI]
    public interface IIsolatedRecordAnalyzer<in TMajor> : IAnalyzer
        where TMajor : IMajorRecordGetter
    {
        MajorRecordAnalyzerResult? AnalyzeRecord(IRecordAnalyzerParams<TMajor> majorRecord);
    }
}
