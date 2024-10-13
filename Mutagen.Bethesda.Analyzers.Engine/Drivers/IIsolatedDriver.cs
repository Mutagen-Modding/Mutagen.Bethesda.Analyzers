namespace Mutagen.Bethesda.Analyzers.Drivers;

public interface IIsolatedDriver : IDriver
{
    Task Drive(IsolatedDriverParams driverParams);
}
