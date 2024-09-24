using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins.Cache;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Reporting.Handlers;

public record struct HandlerParameters(
    ILinkCache LinkCache);

public interface IReportHandler
{
    void Dropoff(
        HandlerParameters parameters,
        IModGetter sourceMod,
        IMajorRecordGetter majorRecord,
        ITopic topic);

    void Dropoff(
        HandlerParameters parameters,
        ITopic topic);
}
