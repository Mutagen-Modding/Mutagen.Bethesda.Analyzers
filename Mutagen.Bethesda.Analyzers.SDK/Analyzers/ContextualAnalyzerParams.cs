using Mutagen.Bethesda.Plugins.Cache;
using Mutagen.Bethesda.Plugins.Order;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.SDK.Analyzers;

public readonly struct ContextualAnalyzerParams
{
    public readonly ILinkCache LinkCache;
    public readonly ILoadOrderGetter<IModListingGetter<IModGetter>> LoadOrder;

    internal ContextualAnalyzerParams(ILinkCache linkCache, ILoadOrderGetter<IModListingGetter<IModGetter>> loadOrder)
    {
        LinkCache = linkCache;
        LoadOrder = loadOrder;
    }
}
