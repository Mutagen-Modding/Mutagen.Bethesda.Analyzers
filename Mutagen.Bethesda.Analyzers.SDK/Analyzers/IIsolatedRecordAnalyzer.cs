using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.SDK.Analyzers;

public interface IIsolatedRecordAnalyzer<TMajor> : IAnalyzer
    where TMajor : IMajorRecordGetter
{
    RecordAnalyzerResult? AnalyzeRecord(IsolatedRecordAnalyzerParams<TMajor> param);
}

public interface IIsolatedRecordFixer<TMajor>
    where TMajor : IMajorRecord
{
    TopicDefinition HandledTopic { get; }
    void FixRecord(IsolatedRecordFixerParams<TMajor> param);
}

public interface IIsolatedRecordFixableAnalyzer<TMajor, TMajorGetter> :
    IIsolatedRecordFixer<TMajor>,
    IIsolatedRecordAnalyzer<TMajorGetter>
    where TMajor : IMajorRecord, TMajorGetter
    where TMajorGetter : IMajorRecordGetter
{
}
