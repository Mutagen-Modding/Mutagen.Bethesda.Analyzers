namespace Mutagen.Bethesda.Analyzers.Drivers
{
    public interface IDriver
    {
        bool Applicable { get; }
        void Drive(DriverParams driverParams);
    }
}
