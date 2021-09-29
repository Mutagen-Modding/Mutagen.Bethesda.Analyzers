using System.IO.Abstractions;
using Autofac;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Mutagen.Bethesda.Analyzers.Autofac;
using Mutagen.Bethesda.Analyzers.Engines;
using Mutagen.Bethesda.Analyzers.Reporting.Console;
using Mutagen.Bethesda.Analyzers.Skyrim;
using Mutagen.Bethesda.Environments.DI;

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
            builder.RegisterInstance(new GameReleaseInjection(GameRelease.SkyrimSE))
                .AsImplementedInterfaces();

            var container = builder.Build();

            var engine = container.Resolve<ContextualEngine>();
            var reporter = container.Resolve<ConsoleReporter>();

            engine.Run(reporter);
        }
    }
}
