using System.Collections.Generic;
using JetBrains.Annotations;
using Mutagen.Bethesda.Analyzers.SDK.Topics;

namespace Mutagen.Bethesda.Analyzers.SDK.Results
{
    [PublicAPI]
    public class MajorRecordAnalyzerResult : IAnalyzerResult<RecordTopic>
    {
        private List<RecordTopic> _topics = new();
        public IReadOnlyCollection<RecordTopic> Topics => _topics;

        public void AddTopic(RecordTopic topic)
        {
            _topics.Add(topic);
        }
    }
}
