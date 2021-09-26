using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.SDK.Analyzers
{
    public interface IContextualRecordAnalyzer<in TMajor> : IAnalyzer
        where TMajor : IMajorRecordGetter
    {
        MajorRecordAnalyzerResult? AnalyzeRecord(IContextualRecordAnalyzerParams<TMajor> majorRecord);
    }
}
