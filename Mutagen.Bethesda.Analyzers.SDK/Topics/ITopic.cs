using JetBrains.Annotations;

namespace Mutagen.Bethesda.Analyzers.SDK.Topics
{
    [PublicAPI]
    public interface ITopic
    {
        TopicDefinition TopicDefinition { get; }
        string FormattedMessage { get; }
        Severity Severity { get; set; }
    }
}
