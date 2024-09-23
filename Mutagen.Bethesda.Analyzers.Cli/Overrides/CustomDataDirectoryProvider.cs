using Mutagen.Bethesda.Environments.DI;
using Noggog;
namespace Mutagen.Bethesda.Analyzers.Cli.Overrides;

public class CustomDataDirectoryProvider(string path) : IDataDirectoryProvider

{
    public DirectoryPath Path { get; } = path;
}
