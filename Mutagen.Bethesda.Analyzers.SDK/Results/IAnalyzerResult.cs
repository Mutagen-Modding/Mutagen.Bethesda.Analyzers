using System.Collections.Generic;
using JetBrains.Annotations;
using Mutagen.Bethesda.Analyzers.SDK.Errors;

namespace Mutagen.Bethesda.Analyzers.SDK.Results
{
    [PublicAPI]
    public interface IAnalyzerResult<TError> where TError : IError
    {
        IReadOnlyCollection<TError> Errors { get; }

        void AddError(TError error);
    }
}
