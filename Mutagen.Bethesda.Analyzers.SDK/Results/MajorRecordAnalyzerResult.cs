using System.Collections.Generic;
using JetBrains.Annotations;
using Mutagen.Bethesda.Analyzers.SDK.Errors;

namespace Mutagen.Bethesda.Analyzers.SDK.Results
{
    [PublicAPI]
    public class MajorRecordAnalyzerResult : IAnalyzerResult<RecordError>
    {
        private List<RecordError> _errors = new();
        public IReadOnlyCollection<RecordError> Errors => _errors;

        public void AddError(RecordError error)
        {
            _errors.Add(error);
        }
    }
}
