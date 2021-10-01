using JetBrains.Annotations;

namespace Mutagen.Bethesda.Analyzers.SDK.Topics
{
    [PublicAPI]
    public record RecordFrameTopic(IFormattedTopicDefinition FormattedTopicDefinition) : ITopic
    {
        public static RecordFrameTopic Create(IFormattedTopicDefinition formattedTopicDefinition)
        {
            return new RecordFrameTopic(formattedTopicDefinition);
        }
    }
}
