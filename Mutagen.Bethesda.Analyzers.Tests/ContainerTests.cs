using Autofac;
using Mutagen.Bethesda.Analyzers.Engines;
using Mutagen.Bethesda.Analyzers.Testing;
using Mutagen.Bethesda.Environments.DI;
using Xunit;

namespace Mutagen.Bethesda.Analyzers.Tests
{
    public class ContainerTests
    {
        [Fact]
        public void ResolvesIsolatedEngine()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<TestModule>();
            builder.RegisterInstance(new GameReleaseInjection(GameRelease.SkyrimSE))
                .AsImplementedInterfaces();
            var container = builder.Build();

            container.Resolve<IsolatedEngine>();
        }

        [Fact]
        public void ResolvesContextualEngine()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<TestModule>();
            builder.RegisterInstance(new GameReleaseInjection(GameRelease.SkyrimSE))
                .AsImplementedInterfaces();
            var container = builder.Build();

            container.Resolve<ContextualEngine>();
        }
    }
}
