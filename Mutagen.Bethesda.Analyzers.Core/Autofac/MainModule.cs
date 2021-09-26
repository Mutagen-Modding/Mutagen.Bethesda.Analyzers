using System.IO.Abstractions;
using System.Linq;
using Autofac;
using Loqui;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Mutagen.Bethesda.Analyzers.Drivers;
using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Autofac;
using Noggog;

namespace Mutagen.Bethesda.Analyzers.Autofac
{
    public class MainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(new FileSystem())
                .As<IFileSystem>();
            builder.RegisterGeneric(typeof(NullLogger<>))
                .As(typeof(ILogger<>))
                .SingleInstance();

            builder.RegisterModule<MutagenModule>();
            builder.RegisterAssemblyTypes(typeof(IModDriver).Assembly)
                .AsImplementedInterfaces()
                .AsSelf();

            foreach (var analyzerType in TypeExt.GetInheritingFromGenericInterface(
                typeof(IIsolatedRecordAnalyzer<>),
                loadAssemblies: true)
                .Select(x => x.Key.GetGenericArguments()[0])
                .Select(x => LoquiRegistration.GetRegister(x).GetterType)
                .Distinct())
            {
                builder.RegisterType(typeof(MajorRecordDriver<>).MakeGenericType(analyzerType))
                    .As<IModDriver>();
            }
        }
    }
}
