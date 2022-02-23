using System;
using System.Diagnostics;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Loqui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using Mutagen.Bethesda.Analyzers.Autofac;
using Mutagen.Bethesda.Analyzers.Cli.Args;
using Mutagen.Bethesda.Analyzers.Engines;
using Mutagen.Bethesda.Analyzers.Reporting;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Analyzers.Skyrim;
using Mutagen.Bethesda.Environments.DI;
using Noggog;
using Noggog.Autofac.Modules;

namespace Mutagen.Bethesda.Analyzers.Cli
{
    public static class RunAnalyzers
    {
        public static async Task<int> Run(RunAnalyzersCommand command)
        {
            var container = GetContainer(command);

            var engine = container.Resolve<ContextualEngine>();

            PrintTopics(command, engine);

            engine.Run();

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
            var services = new ServiceCollection();
            services.AddLogging(x => x.AddConsole());

            var builder = new ContainerBuilder();

            builder.RegisterDecorator<MinimumSeverityFilter, IReportDropbox>();
            builder.RegisterDecorator<SeverityAdjuster, IReportDropbox>();

            builder.Populate(services);
            builder.RegisterInstance(new FileSystem())
                .As<IFileSystem>();
            builder.RegisterModule<MainModule>();
            builder.RegisterInstance(new GameReleaseInjection(command.GameRelease))
                .AsImplementedInterfaces();
            builder.RegisterType<ConsoleReporter>().As<IReportDropbox>();
            builder.RegisterInstance(command).AsImplementedInterfaces();

            // Add Skyrim Analyzers
            builder.RegisterAssemblyTypes(typeof(MissingAssetsAnalyzer).Assembly)
                .AsImplementedInterfaces();

            return builder.Build();
        }
    }
}
