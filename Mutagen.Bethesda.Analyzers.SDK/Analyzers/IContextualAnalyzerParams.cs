using Mutagen.Bethesda.Plugins.Cache;
using Mutagen.Bethesda.Plugins.Order;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.SDK.Analyzers
{
    public interface IContextualAnalyzerParams
    {
        public ILinkCache LinkCache { get; }
        public ILoadOrderGetter<IModListingGetter<IModGetter>> LoadOrder { get; }
    }
}
