using System;
using JetBrains.Annotations;

namespace Mutagen.Bethesda.Analyzers.SDK.Topics
{
    [PublicAPI]
    public partial record TopicDefinition(
        string Id,
        string Title,
        Severity Severity,
        string? MessageFormat = null,
        Uri? InformationUri = null) : ITopicDefinition
    {
        public static TopicDefinition FromDiscussion(
            int id,
            string title,
            Severity severity,
            string discussionsUri)
        {
            return new TopicDefinition(
                Id: id.ToString(),
                Title: title,
                Severity: severity,
                InformationUri: new Uri($"{discussionsUri.TrimEnd('/')}/{id.ToString()}"));
        }

        public IFormattedTopicDefinition Format()
        {
            return new FormattedTopicDefinition(
                this,
                MessageFormat ?? Title);
        }

        public override string ToString() => this.ToShortString();
    }

    [PublicAPI]
    public record TopicDefinition<T1>(
        string Id,
        string Title,
        string MessageFormat,
        Severity Severity,
        Uri? InformationUri = null) : ITopicDefinition
    {
        public IFormattedTopicDefinition Format(T1 item1)
        {
            return new FormattedTopicDefinition<T1>(this, item1);
        }

        public override string ToString() => this.ToShortString();
    }

    [PublicAPI]
    public record TopicDefinition<T1, T2>(
        string Id,
        string Title,
        string MessageFormat,
        Severity Severity,
        Uri? InformationUri = null) : ITopicDefinition
    {
        public IFormattedTopicDefinition Format(T1 item1, T2 item2)
        {
            return new FormattedTopicDefinition<T1, T2>(this, item1, item2);
        }

        public override string ToString() => this.ToShortString();
    }

    [PublicAPI]
    public record TopicDefinition<T1, T2, T3>(
        string Id,
        string Title,
        string MessageFormat,
        Severity Severity,
        Uri? InformationUri = null) : ITopicDefinition
    {
        public IFormattedTopicDefinition Format(T1 item1, T2 item2, T3 item3)
        {
            return new FormattedTopicDefinition<T1, T2, T3>(this, item1, item2, item3);
        }

        public override string ToString() => this.ToShortString();
    }

    [PublicAPI]
    public record TopicDefinition<T1, T2, T3, T4>(
        string Id,
        string Title,
        string MessageFormat,
        Severity Severity,
        Uri? InformationUri = null) : ITopicDefinition
    {
        public IFormattedTopicDefinition Format(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            return new FormattedTopicDefinition<T1, T2, T3, T4>(
                this, item1, item2, item3, item4);
        }

        public override string ToString() => this.ToShortString();
    }
}
