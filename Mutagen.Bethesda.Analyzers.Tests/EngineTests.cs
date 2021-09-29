using System;
using Mutagen.Bethesda.Analyzers.Drivers;
using Mutagen.Bethesda.Analyzers.Engines;
using Mutagen.Bethesda.Analyzers.Reporting;
using Mutagen.Bethesda.Analyzers.Testing.AutoFixture;
using Mutagen.Bethesda.Environments;
using Mutagen.Bethesda.Environments.DI;
using Mutagen.Bethesda.Plugins.Order;
using Mutagen.Bethesda.Plugins.Records;
using NSubstitute;
using Xunit;

namespace Mutagen.Bethesda.Analyzers.Tests
{
    public class EngineTests
    {
        [Theory, NSubData]
        public void IsolatedEngineCallsRecordAnalyzers(
            IModGetter modGetter,
            IReportDropbox reportDropbox,
            IIsolatedDriver[] drivers,
            IDriverProvider<IIsolatedDriver> driverProvider)
        {
            driverProvider.Drivers.Returns(drivers);
            var engine = new IsolatedEngine(driverProvider);

            engine.RunOn(modGetter, reportDropbox);

            foreach (var driver in drivers)
            {
                driver.Received(1).Drive(Arg.Is<IsolatedDriverParams>(
                    x => x.ReportDropbox == reportDropbox
                         && x.TargetMod == modGetter));
            }
        }

        [Theory, NSubData]
        public void ContextualEngineCallsIsolatedRecordAnalyzers(
            IModGetter modA,
            IModGetter modB,
            IReportDropbox reportDropbox,
            IIsolatedDriver[] drivers,
            IGameEnvironmentState gameEnv,
            IGameEnvironmentProvider gameEnvironmentProvider,
            IDriverProvider<IContextualDriver> contextualProvider,
            IDriverProvider<IIsolatedDriver> isolatedProvider)
        {
            var loadOrder = new LoadOrder<IModListingGetter<IModGetter>>();
            loadOrder.Add(new ModListing<IModGetter>(modA));
            loadOrder.Add(new ModListing<IModGetter>(modB));

            gameEnv.LoadOrder.Returns(loadOrder);
            gameEnvironmentProvider.Construct().ReturnsForAnyArgs(gameEnv);
            isolatedProvider.Drivers.Returns(drivers);

            var engine = new ContextualEngine(
                gameEnvironmentProvider,
                contextualProvider,
                isolatedProvider);

            engine.Run(reportDropbox);

            foreach (var driver in drivers)
            {
                driver.Received(1).Drive(Arg.Is<IsolatedDriverParams>(
                    x => x.ReportDropbox == reportDropbox
                         && x.TargetMod == modA));
                driver.Received(1).Drive(Arg.Is<IsolatedDriverParams>(
                    x => x.ReportDropbox == reportDropbox
                         && x.TargetMod == modB));
            }
        }

        [Theory, NSubData]
        public void ContextualEngineCallsContextualRecordAnalyzers(
            IModGetter modA,
            IModGetter modB,
            IReportDropbox reportDropbox,
            IContextualDriver[] drivers,
            IGameEnvironmentState gameEnv,
            IGameEnvironmentProvider gameEnvironmentProvider,
            IDriverProvider<IContextualDriver> contextualProvider,
            IDriverProvider<IIsolatedDriver> isolatedProvider)
        {
            var loadOrder = new LoadOrder<IModListingGetter<IModGetter>>();
            loadOrder.Add(new ModListing<IModGetter>(modA));
            loadOrder.Add(new ModListing<IModGetter>(modB));

            gameEnv.LoadOrder.Returns(loadOrder);
            gameEnvironmentProvider.Construct().ReturnsForAnyArgs(gameEnv);
            contextualProvider.Drivers.Returns(drivers);

            var engine = new ContextualEngine(
                gameEnvironmentProvider,
                contextualProvider,
                isolatedProvider);

            engine.Run(reportDropbox);

            foreach (var driver in drivers)
            {
                driver.Received(1).Drive(Arg.Is<ContextualDriverParams>(
                    x => x.ReportDropbox == reportDropbox
                         && x.LoadOrder == loadOrder));
            }
        }
    }
}
