using Autofac;
using Mutagen.Bethesda.Analyzers.Autofac;
using Mutagen.Bethesda.Analyzers.Reporting;

namespace Mutagen.Bethesda.Analyzers.Cli.Modules;

public class RunAnalyzerModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterDecorator<MinimumSeverityFilter, IReportDropbox>();
        builder.RegisterDecorator<SeverityAdjuster, IReportDropbox>();
        builder.RegisterDecorator<CsvDropbox, IReportDropbox>();
        builder.RegisterModule<MainModule>();
    }
}
