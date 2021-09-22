using JetBrains.Annotations;

namespace Mutagen.Bethesda.Analyzers.SDK.Analyzers
{
    [PublicAPI]
    public interface IAnalyzer
    {
        string Author { get; }
        string Description { get; }
    }
}
