using Loqui;

namespace Mutagen.Bethesda.Analyzers.SDK.Topics
{
    public static class TopicPrinting
    {
        public static string ToShortString(this ITopicDefinition topic)
        {
            return $"[{topic.Id}][{topic.Severity.ToShortString()}] {topic.Title}";
        }

        public static void Append(this ITopicDefinition topic, FileGeneration fg)
        {
            fg.AppendLine(topic.ToShortString());
            using (new DepthWrapper(fg))
            {
                if (topic.InformationUri != null)
                {
                    fg.AppendLine(topic.InformationUri.ToString());
                }
            }
        }
    }
}
