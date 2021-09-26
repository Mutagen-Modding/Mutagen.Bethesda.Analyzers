using System.Collections.Generic;
using System.Linq;

namespace Mutagen.Bethesda.Analyzers.Drivers
{
    public interface IModDriverProvider
    {
        IEnumerable<IModDriver> Drivers { get; }
    }

    public class InjectionDriverProvider : IModDriverProvider
    {
        public IEnumerable<IModDriver> Drivers { get; }

        public InjectionDriverProvider(IEnumerable<IModDriver> drivers)
        {
            Drivers = drivers
                .Where(x => x.Applicable)
                .ToArray();
        }
    }
}
