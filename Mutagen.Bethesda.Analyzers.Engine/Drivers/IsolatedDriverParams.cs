using Mutagen.Bethesda.Analyzers.Reporting;
using Mutagen.Bethesda.Plugins.Cache;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Drivers
{
    public readonly struct IsolatedDriverParams
    {
        public readonly ILinkCache LinkCache;
        public readonly IReportDropbox ReportDropbox;
        public readonly IModGetter TargetMod;

        public IsolatedDriverParams(ILinkCache linkCache, IReportDropbox reportDropbox, IModGetter targetMod)
        {
            LinkCache = linkCache;
            ReportDropbox = reportDropbox;
            TargetMod = targetMod;
        }
    }
}
