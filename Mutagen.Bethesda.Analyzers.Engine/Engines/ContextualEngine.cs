using System.Linq;
using Mutagen.Bethesda.Analyzers.Drivers;
using Mutagen.Bethesda.Analyzers.Reporting;
using Mutagen.Bethesda.Plugins.Order;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Engines
{
    public class ContextualEngine
    {
        private readonly IContextualDriver[] _contextualModDrivers;
        private readonly IIsolatedDriver[] _isolatedModDrivers;

        public ContextualEngine(
            IModDriverProvider<IContextualDriver> contextualModDrivers,
            IModDriverProvider<IIsolatedDriver> isolatedModDrivers)
        {
            _contextualModDrivers = contextualModDrivers.Drivers
                .ToArray();
            _isolatedModDrivers = isolatedModDrivers.Drivers
                .ToArray();
        }

        public void RunOn(ILoadOrderGetter<IModListingGetter<IModGetter>> loadOrder, IReportDropbox reportDropbox)
        {
            var contextualParam = new ContextualDriverParams(
                loadOrder.ToUntypedImmutableLinkCache(),
                loadOrder,
                reportDropbox);
            var isolatedParam = new IsolatedDriverParams(
                loadOrder.ToUntypedImmutableLinkCache(),
                reportDropbox,
                null!);

            foreach (var listing in loadOrder.ListedOrder)
            {
                if (listing.Mod == null) continue;

                isolatedParam = isolatedParam with { TargetMod = listing.Mod };
                foreach (var driver in _isolatedModDrivers)
                {
                    driver.Drive(isolatedParam);
                }
            }

            foreach (var driver in _contextualModDrivers)
            {
                driver.Drive(contextualParam);
            }
        }
    }
}
