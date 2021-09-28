using System.Collections.Generic;
using System.Linq;

namespace Mutagen.Bethesda.Analyzers.Drivers
{
    public interface IModDriverProvider<TDriver>
        where TDriver : IDriver
    {
        IEnumerable<TDriver> Drivers { get; }
    }

    public class InjectionDriverProvider<TDriver> : IModDriverProvider<TDriver>
        where TDriver : IDriver
    {
        public IEnumerable<TDriver> Drivers { get; }

        public InjectionDriverProvider(IEnumerable<TDriver> drivers)
        {
            Drivers = drivers
                .Where(x => x.Applicable)
                .ToArray();
        }
    }
}
