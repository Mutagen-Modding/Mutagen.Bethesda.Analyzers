using System.Linq;
using Mutagen.Bethesda.Analyzers.Drivers;
using Mutagen.Bethesda.Analyzers.Reporting;
using Mutagen.Bethesda.Environments.DI;

namespace Mutagen.Bethesda.Analyzers.Engines
{
    public class ContextualEngine
    {
        private readonly IGameEnvironmentProvider _envGetter;
        private readonly IContextualDriver[] _contextualModDrivers;
        private readonly IIsolatedDriver[] _isolatedModDrivers;

        public ContextualEngine(
            IGameEnvironmentProvider envGetter,
            IDriverProvider<IContextualDriver> contextualDrivers,
            IDriverProvider<IIsolatedDriver> isolatedDrivers)
        {
            _envGetter = envGetter;
            _contextualModDrivers = contextualDrivers.Drivers
                .ToArray();
            _isolatedModDrivers = isolatedDrivers.Drivers
                .ToArray();
        }

        public void Run(IReportDropbox reportDropbox)
        {
            using var env = _envGetter.Construct();

            var contextualParam = new ContextualDriverParams(
                env.LinkCache,
                env.LoadOrder,
                reportDropbox);

            foreach (var listing in env.LoadOrder.ListedOrder)
            {
                if (listing.Mod == null) continue;

                foreach (var driver in _isolatedModDrivers)
                {
                    driver.Drive(new IsolatedDriverParams(
                        listing.Mod.ToUntypedImmutableLinkCache(),
                        reportDropbox,
                        listing.Mod));
                }
            }

            foreach (var driver in _contextualModDrivers)
            {
                driver.Drive(contextualParam);
            }
        }
    }
}
