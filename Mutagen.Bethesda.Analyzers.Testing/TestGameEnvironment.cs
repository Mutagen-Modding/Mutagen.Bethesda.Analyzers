using Mutagen.Bethesda.Environments;
using Mutagen.Bethesda.Plugins.Cache;
using Mutagen.Bethesda.Plugins.Order;
using Mutagen.Bethesda.Plugins.Records;
using Noggog;

namespace Mutagen.Bethesda.Analyzers.Testing;

public record TestGameEnvironment : IGameEnvironment
{
    public required DirectoryPath DataFolderPath { get; init; }
    public required GameRelease GameRelease { get; init; }
    public required FilePath LoadOrderFilePath { get; init; }
    public required FilePath? CreationClubListingsFilePath { get; init; }
    public required ILoadOrderGetter<IModListingGetter<IModGetter>> LoadOrder { get; init; }
    public required ILinkCache LinkCache { get; init; }

    public void Dispose()
    {
    }
}
