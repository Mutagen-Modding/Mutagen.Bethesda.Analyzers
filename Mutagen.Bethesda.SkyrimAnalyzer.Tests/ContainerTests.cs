using System.Linq;
using Autofac;
using Mutagen.Bethesda.Analyzers.Drivers;
using FluentAssertions;
using Mutagen.Bethesda.Analyzers.Testing;
using Xunit;

namespace Mutagen.Bethesda.SkyrimAnalyzer.Tests
{
    public class ContainerTests
    {
        [Fact]
        public void ResolvesMajorRecordDriver()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<TestModule>();
            var container = builder.Build();

            var drivers = container.Resolve<IModDriver[]>();
            drivers
                .Any(x => typeof(RecordDriver<>).IsAssignableFrom(x.GetType().GetGenericTypeDefinition()))
                .Should().BeTrue();
        }
    }
}
