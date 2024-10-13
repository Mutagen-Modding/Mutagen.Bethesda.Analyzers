using Mutagen.Bethesda.Environments;
using Mutagen.Bethesda.Environments.DI;
using Mutagen.Bethesda.Plugins.Cache;

namespace Mutagen.Bethesda.Analyzers.Testing;

public class TestGameEnvironmentProvider : IGameEnvironmentProvider
{
    private readonly IGameEnvironment _gameEnvironment;

    public TestGameEnvironmentProvider(IGameEnvironment gameEnvironment)
    {
        _gameEnvironment = gameEnvironment;
    }

    public IGameEnvironment Construct(LinkCachePreferences? linkCachePrefs = null)
    {
        return _gameEnvironment;
    }
}
