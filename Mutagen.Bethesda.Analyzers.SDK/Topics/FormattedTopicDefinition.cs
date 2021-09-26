using JetBrains.Annotations;

namespace Mutagen.Bethesda.Analyzers.SDK.Topics
{
    [PublicAPI]
    public class FormattedTopicDefinition
    {
        public readonly ITopicDefinition TopicDefinition;
        public readonly string FormattedMessage;

        public FormattedTopicDefinition(ITopicDefinition topicDefinition, string formattedMessage)
        {
            TopicDefinition = topicDefinition;
            FormattedMessage = formattedMessage;
        }

        public override string ToString()
        {
            return FormattedMessage;
        }
    }
}
