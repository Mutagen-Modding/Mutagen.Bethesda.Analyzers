using System.Linq;
using Mutagen.Bethesda.Analyzers.Drivers;
using Mutagen.Bethesda.Analyzers.Reporting;
using Mutagen.Bethesda.Plugins.Order;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Engines
{
    public class IsolatedEngine
    {
        private readonly IDriver[] _drivers;

        public IsolatedEngine(IModDriverProvider modDrivers)
        {
            _drivers = modDrivers.Drivers
                .ToArray();
        }

        public void RunOn(IModGetter modGetter, IReportDropbox reportDropbox)
        {
            var lo = new LoadOrder<IModListing<IModGetter>>();
            lo.Add(new ModListing<IModGetter>(modGetter));
            var driverParams = new DriverParams(
                modGetter.ToUntypedImmutableLinkCache(),
                lo,
                reportDropbox,
                modGetter);
            foreach (var driver in _drivers)
            {
                driver.Drive(driverParams);
            }
        }
    }
}
