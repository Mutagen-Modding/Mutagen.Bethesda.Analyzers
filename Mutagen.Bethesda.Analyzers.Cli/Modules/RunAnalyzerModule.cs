using Autofac;
using Mutagen.Bethesda.Analyzers.Autofac;
using Mutagen.Bethesda.Analyzers.Cli.Args;
using Mutagen.Bethesda.Analyzers.Reporting.Drops;
using Mutagen.Bethesda.Analyzers.Reporting.Handlers;
using Mutagen.Bethesda.Analyzers.SDK.Drops;

namespace Mutagen.Bethesda.Analyzers.Cli.Modules;

public class RunAnalyzerModule(RunAnalyzersCommand? command) : Module
{
    public RunAnalyzerModule() : this(null) {}

    protected override void Load(ContainerBuilder builder)
    {
        if (command?.OutputFilePath is not null)
        {
            builder.RegisterType<CsvReportHandler>().AsImplementedInterfaces();
            builder.RegisterInstance(new CsvInputs(command.OutputFilePath)).AsSelf().AsImplementedInterfaces();
        }

        builder.RegisterDecorator<SeverityAdjuster, IReportDropbox>();
        builder.RegisterDecorator<MinimumSeverityFilter, IReportDropbox>();
        builder.RegisterDecorator<EditorIdEnricher, IReportDropbox>();
        builder.RegisterDecorator<DisallowedParametersChecker, IReportDropbox>();
        builder.RegisterType<PassToHandlerReportDropbox>().AsImplementedInterfaces();
        builder.RegisterModule<MainModule>();
    }
}
