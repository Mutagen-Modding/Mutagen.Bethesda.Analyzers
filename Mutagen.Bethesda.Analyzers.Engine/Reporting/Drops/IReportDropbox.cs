using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Reporting.Drops;

public interface IReportDropbox
{
    void Dropoff(
        ReportContextParameters parameters,
        IModGetter sourceMod,
        IMajorRecordGetter majorRecord,
        ITopic topic);

    void Dropoff(
        ReportContextParameters parameters,
        ITopic topic);
}
