using JetBrains.Annotations;

namespace Mutagen.Bethesda.Analyzers.SDK.Topics
{
    [PublicAPI]
    public interface ITopic
    {
        FormattedTopicDefinition FormattedTopicDefinition { get; }
    }
}
