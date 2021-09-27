using System.Collections.Generic;
using System.Linq;

namespace Mutagen.Bethesda.Analyzers.Drivers
{
    public interface IModDriverProvider
    {
        IEnumerable<IDriver> Drivers { get; }
    }

    public class InjectionDriverProvider : IModDriverProvider
    {
        public IEnumerable<IDriver> Drivers { get; }

        public InjectionDriverProvider(IEnumerable<IDriver> drivers)
        {
            Drivers = drivers
                .Where(x => x.Applicable)
                .ToArray();
        }
    }
}
