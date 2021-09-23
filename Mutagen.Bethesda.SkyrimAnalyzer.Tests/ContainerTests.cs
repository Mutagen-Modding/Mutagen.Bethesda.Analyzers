using System.Linq;
using Autofac;
using Mutagen.Bethesda.Analyzers.Autofac;
using Mutagen.Bethesda.Analyzers.Drivers;
using FluentAssertions;
using Xunit;

namespace Mutagen.Bethesda.SkyrimAnalyzer.Tests
{
    public class ContainerTests
    {
        [Fact]
        public void ResolvesMajorRecordDriver()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<MainModule>();
            builder.RegisterAssemblyTypes(typeof(MissingAssetsAnalyzer).Assembly)
                .AsImplementedInterfaces();
            var container = builder.Build();

            var drivers = container.Resolve<IModDriver[]>();
            drivers
                .Any(x => typeof(MajorRecordDriver<>).IsAssignableFrom(x.GetType().GetGenericTypeDefinition()))
                .Should().BeTrue();
        }
    }
}
