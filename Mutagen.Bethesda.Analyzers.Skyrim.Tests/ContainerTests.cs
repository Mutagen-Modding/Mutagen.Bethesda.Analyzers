using System.Linq;
using Autofac;
using Mutagen.Bethesda.Analyzers.Drivers;
using FluentAssertions;
using Mutagen.Bethesda.Analyzers.Testing;
using Xunit;

namespace Mutagen.Bethesda.Analyzers.Skyrim.Tests
{
    public class ContainerTests
    {
        [Fact]
        public void ResolvesMajorRecordDriver()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<TestModule>();
            var container = builder.Build();

            var drivers = container.Resolve<IIsolatedDriver[]>();
            drivers
                .Any(x => typeof(ByTypeDriver<>).IsAssignableFrom(x.GetType().GetGenericTypeDefinition()))
                .Should().BeTrue();
        }
    }
}
