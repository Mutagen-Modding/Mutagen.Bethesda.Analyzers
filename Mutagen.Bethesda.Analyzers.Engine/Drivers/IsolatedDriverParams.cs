using Mutagen.Bethesda.Analyzers.Reporting;
using Mutagen.Bethesda.Plugins.Cache;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Drivers
{
    public interface IIsolatedDriverParams
    {
        ILinkCache LinkCache { get; init; }
        IReportDropbox ReportDropbox { get; init; }
        IModGetter TargetMod { get; init; }
    }

    public record IsolatedDriverParams(
        ILinkCache LinkCache,
        IReportDropbox ReportDropbox,
        IModGetter TargetMod) : IIsolatedDriverParams;
}
