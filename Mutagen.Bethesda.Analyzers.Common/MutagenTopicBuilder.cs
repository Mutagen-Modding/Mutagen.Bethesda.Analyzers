using Mutagen.Bethesda.Analyzers.SDK.Topics;

namespace Mutagen.Bethesda.Analyzers;

public static class MutagenTopicBuilder
{
    public const string Prefix = "A";

    public static TopicDefinition FromDiscussion(
        ushort id,
        string title,
        Severity severity)
    {
        return TopicDefinition.FromDiscussion(Prefix, id, title, severity, Constants.AnalyzersDiscussionUrl);
    }

    public static TopicDefinition DevelopmentTopic(
        string title,
        Severity severity)
    {
        return new TopicDefinition(
            new TopicId($"Dev-{title}", 0),
            title,
            severity);
    }
}
