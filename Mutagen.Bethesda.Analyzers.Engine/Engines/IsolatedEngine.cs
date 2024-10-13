using Mutagen.Bethesda.Analyzers.Drivers;
using Mutagen.Bethesda.Analyzers.SDK.Drops;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Records.DI;
using Noggog.WorkEngine;

namespace Mutagen.Bethesda.Analyzers.Engines;

public interface IIsolatedEngine : IEngine
{
    Task RunOn(ModPath modPath, IReportDropbox reportDropbox, CancellationToken cancel);
}

public class IsolatedEngine : IIsolatedEngine
{
    private readonly IWorkDropoff _workDropoff;
    public IModImporter ModImporter { get; }
    public IDriverProvider<IIsolatedDriver> IsolatedDrivers { get; }

    public IEnumerable<IDriver> Drivers => IsolatedDrivers.Drivers;

    public IsolatedEngine(
        IModImporter modImporter,
        IDriverProvider<IIsolatedDriver> isolatedDrivers,
        IWorkDropoff workDropoff)
    {
        ModImporter = modImporter;
        IsolatedDrivers = isolatedDrivers;
        _workDropoff = workDropoff;
    }

    public async Task RunOn(
        ModPath modPath,
        IReportDropbox reportDropbox,
        CancellationToken cancel)
    {
        if (cancel.IsCancellationRequested) return;
        var mod = ModImporter.Import(modPath);

        var driverParams = new IsolatedDriverParams(
            mod.ToUntypedImmutableLinkCache(),
            reportDropbox,
            mod,
            modPath,
            cancel);

        if (cancel.IsCancellationRequested) return;
        await Task.WhenAll(IsolatedDrivers.Drivers.Select(driver =>
        {
            return _workDropoff.EnqueueAndWait(() =>
            {
                return driver.Drive(driverParams);
            }, cancel);
        }));
    }
}
