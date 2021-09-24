using JetBrains.Annotations;

namespace Mutagen.Bethesda.Analyzers.SDK.Errors
{
    [PublicAPI]
    public class FormattedErrorDefinition
    {
        public readonly ErrorDefinition ErrorDefinition;
        public readonly string FormattedMessage;

        private FormattedErrorDefinition(ErrorDefinition errorDefinition, string formattedMessage)
        {
            ErrorDefinition = errorDefinition;
            FormattedMessage = formattedMessage;
        }

        public static FormattedErrorDefinition Create(ErrorDefinition errorDefinition, params object?[]? formatArgs)
        {
            return new FormattedErrorDefinition(
                errorDefinition,
                formatArgs == null
                    ? errorDefinition.MessageFormat
                    : string.Format(errorDefinition.MessageFormat, formatArgs));
        }

        public override string ToString()
        {
            return FormattedMessage;
        }
    }
}
