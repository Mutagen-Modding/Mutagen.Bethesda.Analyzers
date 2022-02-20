using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Reporting
{
    public interface IReportDropbox
    {
        void Dropoff(
            IModGetter sourceMod,
            IMajorRecordGetter majorRecord,
            RecordAnalyzerResult? result);
    }
}
