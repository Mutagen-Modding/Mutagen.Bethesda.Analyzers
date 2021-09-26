using JetBrains.Annotations;

namespace Mutagen.Bethesda.Analyzers.SDK.Errors
{
    [PublicAPI]
    public class FormattedErrorDefinition
    {
        public readonly IErrorDefinition ErrorDefinition;
        public readonly string FormattedMessage;

        public FormattedErrorDefinition(IErrorDefinition errorDefinition, string formattedMessage)
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
