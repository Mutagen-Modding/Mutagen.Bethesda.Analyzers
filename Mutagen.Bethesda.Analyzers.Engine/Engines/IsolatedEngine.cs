using System.Linq;
using Mutagen.Bethesda.Analyzers.Drivers;
using Mutagen.Bethesda.Analyzers.Reporting;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Engines
{
    public class IsolatedEngine
    {
        private readonly IIsolatedDriver[] _drivers;

        public IsolatedEngine(IModDriverProvider<IIsolatedDriver> isolatedModDrivers)
        {
            _drivers = isolatedModDrivers.Drivers
                .ToArray();
        }

        public void RunOn(IModGetter modGetter, IReportDropbox reportDropbox)
        {
            var driverParams = new IsolatedDriverParams(
                modGetter.ToUntypedImmutableLinkCache(),
                reportDropbox,
                modGetter);
            foreach (var driver in _drivers)
            {
                driver.Drive(driverParams);
            }
        }
    }
}
