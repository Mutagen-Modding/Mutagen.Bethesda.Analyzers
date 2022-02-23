using JetBrains.Annotations;

namespace Mutagen.Bethesda.Analyzers.SDK.Topics
{
    [PublicAPI]
    public record RecordFrameTopic(IFormattedTopicDefinition _formattedTopicDefinition) : ITopic
    {
        private readonly IFormattedTopicDefinition _formattedTopicDefinition = _formattedTopicDefinition;

        public static RecordFrameTopic Create(IFormattedTopicDefinition formattedTopicDefinition)
        {
            return new RecordFrameTopic(formattedTopicDefinition);
        }

        public TopicDefinition TopicDefinition => _formattedTopicDefinition.TopicDefinition;
        public string FormattedMessage => _formattedTopicDefinition.FormattedMessage;
        public Severity Severity { get; set; } = _formattedTopicDefinition.TopicDefinition.Severity;
    }
}
