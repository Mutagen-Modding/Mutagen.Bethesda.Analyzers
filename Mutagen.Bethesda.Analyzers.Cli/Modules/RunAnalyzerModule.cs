using Autofac;
using Mutagen.Bethesda.Analyzers.Autofac;
using Mutagen.Bethesda.Analyzers.Cli.Args;
using Mutagen.Bethesda.Analyzers.Reporting;

namespace Mutagen.Bethesda.Analyzers.Cli.Modules;

public class RunAnalyzerModule(RunAnalyzersCommand? command) : Module
{
    public RunAnalyzerModule() : this(null) {}

    protected override void Load(ContainerBuilder builder)
    {
        if (command?.OutputFilePath is not null)
        {
            builder.RegisterDecorator<IReportDropbox>((_, _, dropbox) => new CsvDropbox(dropbox, command.OutputFilePath));
        }

        builder.RegisterDecorator<MinimumSeverityFilter, IReportDropbox>();
        builder.RegisterDecorator<SeverityAdjuster, IReportDropbox>();
        builder.RegisterDecorator<TopicListJoin, IReportDropbox>();
        builder.RegisterDecorator<TopicEnricher, IReportDropbox>();
        builder.RegisterModule<MainModule>();
    }
}
