using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Reporting.Handlers;

public interface IReportHandler
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
