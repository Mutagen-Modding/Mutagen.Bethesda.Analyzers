using Mutagen.Bethesda.Analyzers.Reporting.Handlers;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Reporting.Drops;

public class PassToHandlerReportDropbox : IReportDropbox
{
    private readonly IReportHandler[] _handlers;

    public PassToHandlerReportDropbox(IReportHandler[] handlers)
    {
        _handlers = handlers;

    }

    public void Dropoff(ReportContextParameters parameters, IModGetter sourceMod, IMajorRecordGetter majorRecord, ITopic topic)
    {
        foreach (var handler in _handlers)
        {
            handler.Dropoff(parameters, sourceMod, majorRecord, topic);
        }
    }

    public void Dropoff(ReportContextParameters parameters, ITopic topic)
    {
        foreach (var handler in _handlers)
        {
            handler.Dropoff(parameters, topic);
        }
    }
}
