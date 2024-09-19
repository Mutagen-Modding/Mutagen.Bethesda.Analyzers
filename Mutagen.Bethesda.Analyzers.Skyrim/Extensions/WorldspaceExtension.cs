using Mutagen.Bethesda.Plugins.Cache;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim;

public static class WorldspaceExtension
{
    public static IEnumerable<ILocationGetter> GetWorldLocations(this IWorldspaceGetter world, ILinkCache linkCache)
    {
        var worldLocations = new HashSet<ILocationGetter>();

        var worldLocation = world.Location.TryResolve(linkCache);
        while (worldLocation != null)
        {
            worldLocations.Add(worldLocation);
            worldLocation = worldLocation.ParentLocation.TryResolve(linkCache);
        }

        return worldLocations;
    }

}
