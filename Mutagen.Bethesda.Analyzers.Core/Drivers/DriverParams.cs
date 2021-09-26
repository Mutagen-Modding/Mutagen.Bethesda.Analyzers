using Mutagen.Bethesda.Analyzers.Reporting;
using Mutagen.Bethesda.Plugins.Cache;
using Mutagen.Bethesda.Plugins.Order;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Drivers
{
    public record DriverParams(
        ILinkCache LinkCache,
        ILoadOrderGetter<IModListingGetter<IModGetter>> LoadOrder,
        IReportDropbox ReportDropbox,
        IModGetter TargetMod);
}
