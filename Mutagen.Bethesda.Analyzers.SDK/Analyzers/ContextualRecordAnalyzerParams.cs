using Mutagen.Bethesda.Plugins.Cache;
using Mutagen.Bethesda.Plugins.Order;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.SDK.Analyzers
{
    public readonly struct ContextualRecordAnalyzerParams<TMajor>
        where TMajor : IMajorRecordGetter
    {
        public readonly ILinkCache LinkCache;
        public readonly ILoadOrderGetter<IModListingGetter<IModGetter>> LoadOrder;
        public readonly TMajor Record;

        public ContextualRecordAnalyzerParams(ILinkCache linkCache, ILoadOrderGetter<IModListingGetter<IModGetter>> loadOrder, TMajor @record)
        {
            LinkCache = linkCache;
            LoadOrder = loadOrder;
            Record = record;
        }
    }
}
