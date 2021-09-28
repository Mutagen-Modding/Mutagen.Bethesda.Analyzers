using Mutagen.Bethesda.Plugins.Cache;
using Mutagen.Bethesda.Plugins.Order;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.SDK.Analyzers
{
    public interface IContextualRecordAnalyzerParams<out TMajor> : IContextualAnalyzerParams
        where TMajor : IMajorRecordGetter
    {
        public TMajor Record { get; }
    }

    public record ContextualRecordAnalyzerParams<TMajor>(
        ILinkCache LinkCache,
        ILoadOrderGetter<IModListingGetter<IModGetter>> LoadOrder) : IContextualRecordAnalyzerParams<TMajor>
        where TMajor : IMajorRecordGetter
    {
        public TMajor Record { get; init; } = default!;
    }
}
