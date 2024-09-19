using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim;

public static class FurnitureExtension
{
    public static bool IsBed(this IFurnitureGetter furniture)
    {
        return furniture.EditorID is not null && furniture.EditorID.Contains("bed", StringComparison.OrdinalIgnoreCase);
    }
}
