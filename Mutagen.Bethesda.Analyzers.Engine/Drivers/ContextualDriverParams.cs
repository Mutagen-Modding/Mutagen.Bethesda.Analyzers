using Mutagen.Bethesda.Analyzers.Reporting;
using Mutagen.Bethesda.Plugins.Cache;
using Mutagen.Bethesda.Plugins.Order;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Drivers
{
    public interface IContextualDriverParams
    {
        ILinkCache LinkCache { get; init; }
        IReportDropbox ReportDropbox { get; init; }
        ILoadOrderGetter<IModListingGetter<IModGetter>> LoadOrder { get; init; }
    }

    public record ContextualDriverParams(
        ILinkCache LinkCache,
        ILoadOrderGetter<IModListingGetter<IModGetter>> LoadOrder,
        IReportDropbox ReportDropbox) : IContextualDriverParams;
}
