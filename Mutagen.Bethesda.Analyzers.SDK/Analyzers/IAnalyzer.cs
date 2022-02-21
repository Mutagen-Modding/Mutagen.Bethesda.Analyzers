using System.Collections.Generic;
using JetBrains.Annotations;
using Mutagen.Bethesda.Analyzers.SDK.Topics;

namespace Mutagen.Bethesda.Analyzers.SDK.Analyzers
{
    [PublicAPI]
    public interface IAnalyzer
    {
        IEnumerable<TopicDefinition> Topics { get; }
    }
}
