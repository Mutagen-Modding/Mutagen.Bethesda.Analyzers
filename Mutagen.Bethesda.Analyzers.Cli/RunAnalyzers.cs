using System;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Loqui;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Mutagen.Bethesda.Analyzers.Autofac;
using Mutagen.Bethesda.Analyzers.Cli.Args;
using Mutagen.Bethesda.Analyzers.Engines;
using Mutagen.Bethesda.Analyzers.Reporting.Console;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Analyzers.Skyrim;
using Mutagen.Bethesda.Environments.DI;
using Noggog;

namespace Mutagen.Bethesda.Analyzers.Cli
{
    public static class RunAnalyzers
    {
        public static async Task<int> Run(RunAnalyzersCommand command)
        {
            var container = GetContainer(command);

            var engine = container.Resolve<ContextualEngine>();
            var reporter = container.Resolve<ConsoleReporter>();

            PrintTopics(command, engine);

            engine.Run(reporter);

            return 0;
        }

        private static void PrintTopics(RunAnalyzersCommand command, ContextualEngine engine)
        {
            if (!command.PrintTopics) return;

            Console.WriteLine("Topics:");
            var fg = new FileGeneration();
            foreach (var topic in engine.Drivers
                         .SelectMany(d => d.Analyzers)
                         .SelectMany(a => a.Topics)
                         .Distinct(x => x.Id))
            {
                topic.Append(fg);
            }

            foreach (var line in fg)
            {
                Console.WriteLine(line);
            }

            Console.WriteLine();
            Console.WriteLine();
        }

        private static IContainer GetContainer(RunAnalyzersCommand command)
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
            builder.RegisterInstance(new GameReleaseInjection(command.GameRelease))
                .AsImplementedInterfaces();

            return builder.Build();
        }
    }
}
