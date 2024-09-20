using Mutagen.Bethesda.Plugins.Cache;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim;

public static class NpcExtensions
{
    public static IScriptEntryGetter? GetScript(this INpcGetter npc, string name)
    {
        return npc.VirtualMachineAdapter?.Scripts.FirstOrDefault(script => script.Name == name);
    }

    public static bool HasScript(this INpcGetter npc, string name)
    {
        return npc.GetScript(name) is not null;
    }

    public static bool HasFaction(this INpcGetter npc, ILinkCache linkCache, Predicate<string?> stringCompare)
    {
        foreach (var rankPlacement in npc.Factions)
        {
            if (!linkCache.TryResolve<IFactionGetter>(rankPlacement.Faction.FormKey, out var faction)) continue;

            if (stringCompare(faction.EditorID)) return true;
        }

        return false;
    }

    public static bool HasFaction(this INpcGetter npc, ILinkCache linkCache, string editorId)
    {
        return npc.HasFaction(linkCache, npcEditorId => string.Equals(npcEditorId, editorId, StringComparison.OrdinalIgnoreCase));
    }

    public static bool IsGhost(this INpcGetter npc)
    {
        return npc.HasKeyword(FormKeys.SkyrimSE.Skyrim.Keyword.ActorTypeGhost);
    }

    public static bool IsUnique(this INpcGetter npc)
    {
        return npc.Configuration.Flags.HasFlag(NpcConfiguration.Flag.Unique);
    }
}
