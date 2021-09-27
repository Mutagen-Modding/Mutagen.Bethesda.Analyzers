using System.Linq;
using Mutagen.Bethesda.Analyzers.Drivers;
using Mutagen.Bethesda.Analyzers.Reporting;
using Mutagen.Bethesda.Plugins.Order;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Engines
{
    public class ContextualEngine
    {
        private readonly IDriver[] _drivers;

        public ContextualEngine(IModDriverProvider modDrivers)
        {
            _drivers = modDrivers.Drivers
                .ToArray();
        }

        public void RunOn(ILoadOrderGetter<IModListingGetter<IModGetter>> loadOrder, IReportDropbox reportDropbox)
        {
            var param = new DriverParams(
                loadOrder.ToUntypedImmutableLinkCache(),
                loadOrder,
                reportDropbox,
                null!);

            foreach (var listing in loadOrder.ListedOrder)
            {
                if (listing.Mod == null) continue;

                param = param with { TargetMod = listing.Mod };
                foreach (var driver in _drivers)
                {
                    driver.Drive(param);
                }
            }
        }
    }
}
