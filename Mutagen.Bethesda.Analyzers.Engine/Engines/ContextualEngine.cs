using Mutagen.Bethesda.Analyzers.Drivers;
using Mutagen.Bethesda.Analyzers.SDK.Drops;
using Mutagen.Bethesda.Environments.DI;
using Noggog.WorkEngine;

namespace Mutagen.Bethesda.Analyzers.Engines;

public interface IContextualEngine : IEngine
{
    Task Run();
}

public class ContextualEngine : IContextualEngine
{
    private readonly IWorkDropoff _workDropoff;
    public IReportDropbox ReportDropbox { get; }
    public IGameEnvironmentProvider EnvGetter { get; }
    public IDataDirectoryProvider DataDirectoryProvider { get; }
    public IDriverProvider<IContextualDriver> ContextualModDrivers { get; }
    public IDriverProvider<IIsolatedDriver> IsolatedModDrivers { get; }

    public IEnumerable<IDriver> Drivers => ContextualModDrivers.Drivers
        .Concat<IDriver>(IsolatedModDrivers.Drivers);

    public ContextualEngine(
        IGameEnvironmentProvider envGetter,
        IDataDirectoryProvider dataDataDirectoryProvider,
        IDriverProvider<IContextualDriver> contextualDrivers,
        IDriverProvider<IIsolatedDriver> isolatedDrivers,
        IReportDropbox reportDropbox,
        IWorkDropoff workDropoff)
    {
        _workDropoff = workDropoff;
        ReportDropbox = reportDropbox;
        EnvGetter = envGetter;
        DataDirectoryProvider = dataDataDirectoryProvider;
        ContextualModDrivers = contextualDrivers;
        IsolatedModDrivers = isolatedDrivers;
    }

    public async Task Run()
    {
        using var env = EnvGetter.Construct();
        CancellationToken cancel = CancellationToken.None;

        List<Task> toDo = new();

        var isolatedDrivers = IsolatedModDrivers.Drivers;
        if (isolatedDrivers.Count > 0)
        {
            foreach (var listing in env.LoadOrder.ListedOrder)
            {
                if (listing.Mod is null) continue;

                var modPath = Path.Combine(DataDirectoryProvider.Path, listing.ModKey.FileName);

                var isolatedParam = new IsolatedDriverParams(
                    listing.Mod.ToUntypedImmutableLinkCache(),
                    ReportDropbox,
                    listing.Mod,
                    modPath);

                toDo.Add(Task.WhenAll(IsolatedModDrivers.Drivers.Select(driver =>
                {
                    return _workDropoff.EnqueueAndWait(() =>
                    {
                        return driver.Drive(isolatedParam);
                    }, cancel);
                })));
            }
        }

        var contextualParam = new ContextualDriverParams(
            env.LinkCache,
            env.LoadOrder,
            ReportDropbox);

        toDo.Add(Task.WhenAll(ContextualModDrivers.Drivers.Select(driver =>
        {
            return _workDropoff.EnqueueAndWait(() =>
            {
                return driver.Drive(contextualParam);
            }, cancel);
        })));

        await Task.WhenAll(toDo);
    }
}
