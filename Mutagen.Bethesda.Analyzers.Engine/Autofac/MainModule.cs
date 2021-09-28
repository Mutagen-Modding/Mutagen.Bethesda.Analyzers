using Autofac;
using Mutagen.Bethesda.Analyzers.Drivers;
using Mutagen.Bethesda.Analyzers.Engines;
using Mutagen.Bethesda.Autofac;

namespace Mutagen.Bethesda.Analyzers.Autofac
{
    public class MainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<MutagenModule>();
            builder.RegisterAssemblyTypes(typeof(IsolatedEngine).Assembly)
                .AsImplementedInterfaces()
                .AsSelf();
            builder.RegisterGeneric(typeof(InjectionDriverProvider<>))
                .As(typeof(IDriverProvider<>))
                .SingleInstance();
        }
    }
}
