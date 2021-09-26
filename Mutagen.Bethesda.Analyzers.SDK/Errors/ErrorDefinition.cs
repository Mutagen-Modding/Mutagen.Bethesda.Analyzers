using System;
using JetBrains.Annotations;

namespace Mutagen.Bethesda.Analyzers.SDK.Errors
{
    public interface IErrorDefinition
    {
        string Id { get; }
        string Title { get; }
        Severity Severity { get; }
    }

    [PublicAPI]
    public record ErrorDefinition(
        string Id,
        string Title,
        string Message,
        Severity Severity) : IErrorDefinition
    {
        public override string ToString()
        {
            return $"[{Severity.ToShortString()}] [{Id}] {Title}: {Message}";
        }

        public FormattedErrorDefinition Format()
        {
            return new FormattedErrorDefinition(
                this,
                Message);
        }
    }

    [PublicAPI]
    public record ErrorDefinition<T1>(
        string Id,
        string Title,
        string MessageFormat,
        Severity Severity) : IErrorDefinition
    {
        public override string ToString()
        {
            return $"[{Severity.ToShortString()}] [{Id}] {Title}: {MessageFormat}";
        }

        public FormattedErrorDefinition Format(T1 item1)
        {
            return new FormattedErrorDefinition(
                this,
                string.Format(MessageFormat, item1));
        }
    }

    [PublicAPI]
    public record ErrorDefinition<T1, T2>(
        string Id,
        string Title,
        string MessageFormat,
        Severity Severity) : IErrorDefinition
    {
        public override string ToString()
        {
            return $"[{Severity.ToShortString()}] [{Id}] {Title}: {MessageFormat}";
        }

        public FormattedErrorDefinition Format(T1 item1, T2 item2)
        {
            return new FormattedErrorDefinition(
                this,
                string.Format(MessageFormat, item1, item2));
        }
    }

    [PublicAPI]
    public record ErrorDefinition<T1, T2, T3>(
        string Id,
        string Title,
        string MessageFormat,
        Severity Severity) : IErrorDefinition
    {
        public override string ToString()
        {
            return $"[{Severity.ToShortString()}] [{Id}] {Title}: {MessageFormat}";
        }

        public FormattedErrorDefinition Format(T1 item1, T2 item2, T3 item3)
        {
            return new FormattedErrorDefinition(
                this,
                string.Format(MessageFormat, item1, item2, item3));
        }
    }

    [PublicAPI]
    public record ErrorDefinition<T1, T2, T3, T4>(
        string Id,
        string Title,
        string MessageFormat,
        Severity Severity) : IErrorDefinition
    {
        public override string ToString()
        {
            return $"[{Severity.ToShortString()}] [{Id}] {Title}: {MessageFormat}";
        }

        public FormattedErrorDefinition Format(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            return new FormattedErrorDefinition(
                this,
                string.Format(MessageFormat, item1, item2, item3, item4));
        }
    }
}
