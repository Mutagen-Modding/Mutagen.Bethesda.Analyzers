namespace Mutagen.Bethesda.Analyzers.Drivers
{
    public interface IIsolatedDriver : IDriver
    {
        void Drive(IIsolatedDriverParams driverParams);
    }
}
