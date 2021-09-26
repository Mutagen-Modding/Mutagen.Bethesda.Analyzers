using System.IO.Abstractions;
using Autofac;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Mutagen.Bethesda.Analyzers.Autofac;

namespace Mutagen.Bethesda.Analyzers.Testing
{
    public class TestModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<MainModule>();
            builder.RegisterInstance(new FileSystem())
                .As<IFileSystem>();
            builder.RegisterGeneric(typeof(NullLogger<>))
                .As(typeof(ILogger<>))
                .SingleInstance();
            builder.RegisterModule<ReflectionDriverModule>();
        }
    }
}
