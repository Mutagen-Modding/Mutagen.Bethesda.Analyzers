using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Cache;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim;

public static class LocationExtensions
{
    public static bool IsLocationAppliedToInterior(this ILocationGetter location, ILinkCache linkCache)
    {
        // todo with reference cache, this can be optimized
        var interiorLocations = new HashSet<FormKey>();
        foreach (var cell in linkCache.PriorityOrder.WinningOverrides<ICellGetter>())
        {
            if (cell.IsInteriorCell() && !cell.Location.FormKey.IsNull)
            {
                interiorLocations.Add(cell.Location.FormKey);
            }
        }

        return interiorLocations.Contains(location.FormKey);
    }

    public static bool IsSettlementLocation(this ILocationGetter location)
    {
        if (location.Keywords is null) return false;

        var isSettlement = location.Keywords.Any(k =>
            k.FormKey == FormKeys.SkyrimSE.Skyrim.Keyword.LocTypeHabitation.FormKey
            || k.FormKey == FormKeys.SkyrimSE.Skyrim.Keyword.LocTypeHabitationHasInn.FormKey
            || k.FormKey == FormKeys.SkyrimSE.Skyrim.Keyword.LocTypeFarm.FormKey
            || k.FormKey == FormKeys.SkyrimSE.Skyrim.Keyword.LocTypeCity.FormKey
            || k.FormKey == FormKeys.SkyrimSE.Skyrim.Keyword.LocTypeHouse.FormKey
            || k.FormKey == FormKeys.SkyrimSE.Skyrim.Keyword.LocTypeSettlement.FormKey
            || k.FormKey == FormKeys.SkyrimSE.Skyrim.Keyword.LocTypePlayerHouse.FormKey
            || k.FormKey == FormKeys.SkyrimSE.Skyrim.Keyword.LocTypeInn.FormKey
            || k.FormKey == FormKeys.SkyrimSE.Skyrim.Keyword.LocTypeStore.FormKey);

        return isSettlement;
    }

    public static bool IsSettlementHouseLocation(this ILocationGetter location)
    {
        if (location.Keywords is null) return false;

        return location.Keywords.Any(k =>
            k.FormKey == FormKeys.SkyrimSE.Skyrim.Keyword.LocTypeHouse.FormKey
            || k.FormKey == FormKeys.SkyrimSE.Skyrim.Keyword.LocTypePlayerHouse.FormKey
            || k.FormKey == FormKeys.SkyrimSE.Skyrim.Keyword.LocTypeInn.FormKey
            || k.FormKey == FormKeys.SkyrimSE.Skyrim.Keyword.LocTypeStore.FormKey);
    }

    public static bool IsSettlementHouseLocationNotPlayerHome(this ILocationGetter location)
    {
        if (location.Keywords is null) return false;

        if (location.Keywords.Any(k => k.FormKey == FormKeys.SkyrimSE.Skyrim.Keyword.LocTypePlayerHouse.FormKey))
        {
            return false;
        }

        return location.Keywords.Any(k =>
            k.FormKey == FormKeys.SkyrimSE.Skyrim.Keyword.LocTypeHouse.FormKey
            || k.FormKey == FormKeys.SkyrimSE.Skyrim.Keyword.LocTypeInn.FormKey
            || k.FormKey == FormKeys.SkyrimSE.Skyrim.Keyword.LocTypeStore.FormKey);
    }

    public static bool IsInnLocation(this ILocationGetter location)
    {
        if (location.Keywords == null) return false;

        return location.Keywords.Any(k => k.FormKey == FormKeys.SkyrimSE.Skyrim.Keyword.LocTypeInn.FormKey);
    }
}
