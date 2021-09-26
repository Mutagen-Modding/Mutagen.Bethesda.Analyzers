using System;
using JetBrains.Annotations;

namespace Mutagen.Bethesda.Analyzers.SDK.Topics
{
    public interface ITopicDefinition
    {
        string Id { get; }
        string Title { get; }
        Severity Severity { get; }
        Uri? InformationUri { get; }
    }

    [PublicAPI]
    public record TopicDefinition(
        string Id,
        string Title,
        string Message,
        Severity Severity,
        Uri? InformationUri = null) : ITopicDefinition
    {
        public override string ToString()
        {
            return $"[{Severity.ToShortString()}] [{Id}] {Title}: {Message}";
        }

        public FormattedTopicDefinition Format()
        {
            return new FormattedTopicDefinition(
                this,
                Message);
        }
    }

    [PublicAPI]
    public record TopicDefinition<T1>(
        string Id,
        string Title,
        string MessageFormat,
        Severity Severity,
        Uri? InformationUri = null) : ITopicDefinition
    {
        public override string ToString()
        {
            return $"[{Severity.ToShortString()}] [{Id}] {Title}: {MessageFormat}";
        }

        public FormattedTopicDefinition Format(T1 item1)
        {
            return new FormattedTopicDefinition(
                this,
                string.Format(MessageFormat, item1));
        }
    }

    [PublicAPI]
    public record TopicDefinition<T1, T2>(
        string Id,
        string Title,
        string MessageFormat,
        Severity Severity,
        Uri? InformationUri = null) : ITopicDefinition
    {
        public override string ToString()
        {
            return $"[{Severity.ToShortString()}] [{Id}] {Title}: {MessageFormat}";
        }

        public FormattedTopicDefinition Format(T1 item1, T2 item2)
        {
            return new FormattedTopicDefinition(
                this,
                string.Format(MessageFormat, item1, item2));
        }
    }

    [PublicAPI]
    public record TopicDefinition<T1, T2, T3>(
        string Id,
        string Title,
        string MessageFormat,
        Severity Severity,
        Uri? InformationUri = null) : ITopicDefinition
    {
        public override string ToString()
        {
            return $"[{Severity.ToShortString()}] [{Id}] {Title}: {MessageFormat}";
        }

        public FormattedTopicDefinition Format(T1 item1, T2 item2, T3 item3)
        {
            return new FormattedTopicDefinition(
                this,
                string.Format(MessageFormat, item1, item2, item3));
        }
    }

    [PublicAPI]
    public record TopicDefinition<T1, T2, T3, T4>(
        string Id,
        string Title,
        string MessageFormat,
        Severity Severity,
        Uri? InformationUri = null) : ITopicDefinition
    {
        public override string ToString()
        {
            return $"[{Severity.ToShortString()}] [{Id}] {Title}: {MessageFormat}";
        }

        public FormattedTopicDefinition Format(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            return new FormattedTopicDefinition(
                this,
                string.Format(MessageFormat, item1, item2, item3, item4));
        }
    }
}