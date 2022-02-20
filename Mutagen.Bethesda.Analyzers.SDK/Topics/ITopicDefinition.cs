using System;

namespace Mutagen.Bethesda.Analyzers.SDK.Topics
{
    public interface ITopicDefinition
    {
        TopicId Id { get; }
        string Title { get; }
        string MessageFormat { get; }
        Severity Severity { get; }
        Uri? InformationUri { get; }
    }
}
