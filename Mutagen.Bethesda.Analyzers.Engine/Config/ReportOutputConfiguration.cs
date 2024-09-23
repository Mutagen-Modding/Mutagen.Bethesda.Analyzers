namespace Mutagen.Bethesda.Analyzers.Config;

public class ReportOutputConfiguration(string outputFilePath) : IReportOutputConfiguration
{
    public string OutputFilePath { get; set; } = outputFilePath;
}
