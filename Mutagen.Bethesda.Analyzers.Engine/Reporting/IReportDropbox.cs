using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Reporting
{
    public interface IReportDropbox
    {
        void Dropoff<TError>(
            IModGetter sourceMod,
            IMajorRecordGetter majorRecord,
            IAnalyzerResult<TError> result)
            where TError : ITopic;

        void Dropoff<TError>(IAnalyzerResult<TError> result)
            where TError : ITopic;
    }
}
