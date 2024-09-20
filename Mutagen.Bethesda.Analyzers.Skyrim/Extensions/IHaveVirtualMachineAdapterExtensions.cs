using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim;

public static class IHaveVirtualMachineAdapterExtensions
{
    public static IScriptEntryGetter? GetScript(this IHaveVirtualMachineAdapterGetter npc, string name)
    {
        return npc.VirtualMachineAdapter?.Scripts.FirstOrDefault(script => script.Name == name);
    }

    public static bool HasScript(this IHaveVirtualMachineAdapterGetter npc, string name)
    {
        return npc.GetScript(name) is not null;
    }
}
