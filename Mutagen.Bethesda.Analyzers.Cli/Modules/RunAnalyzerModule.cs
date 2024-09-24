using Autofac;
using Mutagen.Bethesda.Analyzers.Autofac;
using Mutagen.Bethesda.Analyzers.Cli.Args;
using Mutagen.Bethesda.Analyzers.Reporting.Drops;
using Mutagen.Bethesda.Analyzers.Reporting.Handlers;

namespace Mutagen.Bethesda.Analyzers.Cli.Modules;

public class RunAnalyzerModule(RunAnalyzersCommand? command) : Module
{
    public RunAnalyzerModule() : this(null) {}

    protected override void Load(ContainerBuilder builder)
    {
        if (command?.OutputFilePath is not null)
        {
            builder.RegisterType<CsvReportHandler>().AsImplementedInterfaces();
        }

        builder.RegisterDecorator<MinimumSeverityFilter, IReportDropbox>();
        builder.RegisterDecorator<SeverityAdjuster, IReportDropbox>();
        builder.RegisterDecorator<TopicListJoin, IReportDropbox>();
        builder.RegisterDecorator<TopicEnricher, IReportDropbox>();
        builder.RegisterType<PassToHandlerReportDropbox>().AsImplementedInterfaces();
        builder.RegisterModule<MainModule>();
    }
}
