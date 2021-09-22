using JetBrains.Annotations;
using Mutagen.Bethesda.Plugins;

namespace Mutagen.Bethesda.Analyzers.SDK.Result
{
    [PublicAPI]
    public readonly struct RecordMessage
    {
        public readonly string Message;
        public readonly FormKey FormKey;

        public RecordMessage(string message, FormKey formKey)
        {
            Message = message;
            FormKey = formKey;
        }

        public override string ToString()
        {
            return $"{Message} ({FormKey.ToString()})";
        }
    }
}
