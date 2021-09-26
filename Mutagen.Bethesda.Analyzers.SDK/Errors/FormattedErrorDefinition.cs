using JetBrains.Annotations;

namespace Mutagen.Bethesda.Analyzers.SDK.Errors
{
    [PublicAPI]
    public class FormattedErrorDefinition
    {
        public readonly ErrorDefinition ErrorDefinition;
        public readonly string FormattedMessage;

        public FormattedErrorDefinition(ErrorDefinition errorDefinition, string formattedMessage)
        {
            ErrorDefinition = errorDefinition;
            FormattedMessage = formattedMessage;
        }

        public override string ToString()
        {
            return FormattedMessage;
        }
    }
}
