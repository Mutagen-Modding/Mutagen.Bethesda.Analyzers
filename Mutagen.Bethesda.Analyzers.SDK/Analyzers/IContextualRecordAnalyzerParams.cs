using Mutagen.Bethesda.Plugins.Cache;
using Mutagen.Bethesda.Plugins.Order;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.SDK.Analyzers
{
    public interface IContextualRecordAnalyzerParams<out TMajor>
        where TMajor : IMajorRecordGetter
    {
        public TMajor Record { get; }
        public ILinkCache LinkCache { get; }
        public ILoadOrderGetter<IModListingGetter<IModGetter>> LoadOrder { get; }
    }

    public record ContextualRecordAnalyzerParams<TMajor>(
        ILinkCache LinkCache,
        ILoadOrderGetter<IModListingGetter<IModGetter>> LoadOrder) : IContextualRecordAnalyzerParams<TMajor>
        where TMajor : IMajorRecordGetter
    {
        public TMajor Record { get; init; } = default!;
    }
}
