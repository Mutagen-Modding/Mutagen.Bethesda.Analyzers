using System.Collections.Generic;
using JetBrains.Annotations;
using Mutagen.Bethesda.Analyzers.SDK.Topics;

namespace Mutagen.Bethesda.Analyzers.SDK.Results
{
    [PublicAPI]
    public interface IAnalyzerResult<TError> where TError : ITopic
    {
        IReadOnlyCollection<TError> Topics { get; }

        void AddTopic(TError error);
    }
}
