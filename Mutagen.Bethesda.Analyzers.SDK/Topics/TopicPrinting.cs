using Loqui;

namespace Mutagen.Bethesda.Analyzers.SDK.Topics
{
    public static class TopicPrinting
    {
        public static string ToShortString(this ITopicDefinition topic)
        {
            return $"[{topic.Severity.ToShortString()}] [{topic.Id}] {topic.Title}";
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
