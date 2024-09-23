using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Order;
using Mutagen.Bethesda.Plugins.Order.DI;
namespace Mutagen.Bethesda.Analyzers.Cli.Overrides;

public class CustomEnabledPluginListingsProvider(string path) : IEnabledPluginListingsProvider

{
    public IEnumerable<ILoadOrderListingGetter> Get()
    {
        foreach (var filePath in Directory.EnumerateFiles(path, "*.esp, *.esm, *.esl"))
        {
            var fileName = Path.GetFileName(filePath);

            yield return new LoadOrderListing(ModKey.FromFileName(fileName), true);
        }
    }
}
