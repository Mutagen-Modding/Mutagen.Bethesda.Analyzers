using System.Collections.Generic;
using JetBrains.Annotations;
using Mutagen.Bethesda.Plugins;

namespace Mutagen.Bethesda.Analyzers.SDK.Result
{
    [PublicAPI]
    public class AnalyzerResult
    {
        private List<RecordMessage> _errors = new();
        public IEnumerable<RecordMessage> Errors => _errors;

        public void AddError(string error, FormKey formKey)
        {
            _errors.Add(new RecordMessage(error, formKey));
        }

        public void AddError(RecordMessage recordMessage)
        {
            _errors.Add(recordMessage);
        }
    }
}
