namespace Mutagen.Bethesda.Analyzers.Drivers;

public interface IContextualDriver : IDriver
{
    Task Drive(ContextualDriverParams driverParams);
}
