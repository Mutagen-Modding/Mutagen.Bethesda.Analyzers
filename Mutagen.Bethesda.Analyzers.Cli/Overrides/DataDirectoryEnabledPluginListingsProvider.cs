using Mutagen.Bethesda.Environments.DI;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Order;
using Mutagen.Bethesda.Plugins.Order.DI;
namespace Mutagen.Bethesda.Analyzers.Cli.Overrides;

public class DataDirectoryEnabledPluginListingsProvider(IDataDirectoryProvider dataDirectoryProvider) : IEnabledPluginListingsProvider
{
    private readonly string[] _formats = [".esp", ".esm", ".esl"];

    public IEnumerable<ILoadOrderListingGetter> Get()
    {
        foreach (var filePath in Directory.EnumerateFiles(dataDirectoryProvider.Path).Where(x => _formats.Contains(Path.GetExtension(x))))
        {
            var fileName = Path.GetFileName(filePath);

            yield return new LoadOrderListing(ModKey.FromFileName(fileName), true);
        }
    }
}
