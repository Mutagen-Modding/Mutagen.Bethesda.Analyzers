using Autofac;
using Mutagen.Bethesda.Autofac;

namespace Mutagen.Bethesda.Analyzers.Autofac
{
    public class MainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<MutagenModule>();
            builder.RegisterAssemblyTypes(typeof(Engine).Assembly)
                .AsImplementedInterfaces()
                .AsSelf();
        }
    }
}
