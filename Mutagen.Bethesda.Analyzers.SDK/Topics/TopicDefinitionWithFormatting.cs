namespace Mutagen.Bethesda.Analyzers.SDK.Topics
{
    public partial record TopicDefinition
    {
        public TopicDefinition<T1> WithFormatting<T1>(string messageFormat)
        {
            return new TopicDefinition<T1>(
                Id: Id,
                Title: Title,
                MessageFormat: messageFormat,
                Severity: Severity,
                InformationUri: InformationUri);
        }

        public TopicDefinition<T1, T2> WithFormatting<T1, T2>(string messageFormat)
        {
            return new TopicDefinition<T1, T2>(
                Id: Id,
                Title: Title,
                MessageFormat: messageFormat,
                Severity: Severity,
                InformationUri: InformationUri);
        }

        public TopicDefinition<T1, T2, T3> WithFormatting<T1, T2, T3>(string messageFormat)
        {
            return new TopicDefinition<T1, T2, T3>(
                Id: Id,
                Title: Title,
                MessageFormat: messageFormat,
                Severity: Severity,
                InformationUri: InformationUri);
        }

        public TopicDefinition<T1, T2, T3, T4> WithFormatting<T1, T2, T3, T4>(string messageFormat)
        {
            return new TopicDefinition<T1, T2, T3, T4>(
                Id: Id,
                Title: Title,
                MessageFormat: messageFormat,
                Severity: Severity,
                InformationUri: InformationUri);
        }
    }
}
