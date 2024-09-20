namespace Mutagen.Bethesda.Analyzers.SDK.Topics;

public interface ITopic
{
    TopicDefinition TopicDefinition { get; }
    string FormattedMessage { get; }
    Severity Severity { get; set; }
    IEnumerable<object?> Items { get; }
}
