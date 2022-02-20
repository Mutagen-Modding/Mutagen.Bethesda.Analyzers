using CommandLine;

namespace Mutagen.Bethesda.Analyzers.Cli.Args
{
    [Verb("run-analyzers", HelpText = "Run analyzers on a game installation")]
    public class RunAnalyzersCommand
    {
        [Option('g', "GameRelease", Required = true, HelpText = "Game Release to target")]
        public GameRelease GameRelease { get; set; }

        [Option("PrintTopics", Required = false, HelpText = "Whether to print the topics being run")]
        public bool PrintTopics { get; set; } = false;
    }
}
