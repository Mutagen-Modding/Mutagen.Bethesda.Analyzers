using System.IO.Abstractions;
using Autofac;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Mutagen.Bethesda.Analyzers.Autofac;
using Mutagen.Bethesda.Analyzers.Reporting.Console;
using Mutagen.Bethesda.Environments;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.SkyrimAnalyzer;

namespace Mutagen.Bethesda.Analyzers.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(new FileSystem())
                .As<IFileSystem>();
            builder.RegisterGeneric(typeof(NullLogger<>))
                .As(typeof(ILogger<>))
                .SingleInstance();
            builder.RegisterModule<MainModule>();
            builder.RegisterModule<ReflectionDriverModule>();
            builder.RegisterAssemblyTypes(typeof(MissingAssetsAnalyzer).Assembly)
                .AsImplementedInterfaces();
            var container = builder.Build();

            var engine = container.Resolve<Engine>();
            var reporter = container.Resolve<ConsoleReporter>();

            using var env = GameEnvironment.Typical.Skyrim(SkyrimRelease.SkyrimSE);
            foreach (var mod in env.LoadOrder.ListedOrder)
            {
                if (mod.Mod == null) continue;
                engine.RunOn(mod.Mod, reporter);
            }
        }
    }
}
