using JetBrains.Annotations;

namespace Mutagen.Bethesda.Analyzers.SDK.Topics
{
    [PublicAPI]
    public interface ITopic
    {
        IFormattedTopicDefinition FormattedTopicDefinition { get; }
    }
}
