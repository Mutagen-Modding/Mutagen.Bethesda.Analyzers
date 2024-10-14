using System.Diagnostics.CodeAnalysis;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.SDK.Analyzers;

// ToDo
// Maybe move this more limited interface to mutagen official

public interface IRecordLookup<TMajorGetter>
    where TMajorGetter : class, IMajorRecordGetter
{
    bool TryResolve(IFormLinkGetter<TMajorGetter> link, [MaybeNullWhen(false)] out TMajorGetter rec);
}

public static class RecordLookupExt
{
    public static bool TryResolve<TMajorGetter>(this IFormLinkGetter<TMajorGetter> link, IRecordLookup<TMajorGetter> lookup, [MaybeNullWhen(false)] out TMajorGetter rec)
        where TMajorGetter : class, IMajorRecordGetter
    {
        throw new NotImplementedException();
    }

    public static TMajorGetter? TryResolve<TMajorGetter>(this IFormLinkGetter<TMajorGetter> link, IRecordLookup<TMajorGetter> lookup)
        where TMajorGetter : class, IMajorRecordGetter
    {
        throw new NotImplementedException();
    }

    public static TMajorGetter Resolve<TMajorGetter>(this IFormLinkGetter<TMajorGetter> link, IRecordLookup<TMajorGetter> lookup)
        where TMajorGetter : class, IMajorRecordGetter
    {
        throw new NotImplementedException();
    }

    public static TMajorGetter Resolve<TMajorGetter>(this IRecordLookup<TMajorGetter> lookup, IFormLinkGetter<TMajorGetter> link)
        where TMajorGetter : class, IMajorRecordGetter
    {
        throw new NotImplementedException();
    }
}
