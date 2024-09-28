using System.Linq.Expressions;
using Mutagen.Bethesda.Analyzers.SDK.Drops;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.SDK.Analyzers;

public readonly struct IsolatedRecordAnalyzerParams<TMajor>
    where TMajor : IMajorRecordGetter
{
    private readonly ModKey _mod;
    public readonly TMajor Record;
    private readonly ReportContextParameters _parameters;
    private readonly IReportDropbox _reportDropbox;

    public IsolatedRecordAnalyzerParams(
        ModKey mod,
        TMajor record,
        ReportContextParameters parameters,
        IReportDropbox reportDropbox)
    {
        _mod = mod;
        Record = record;
        _parameters = parameters;
        _reportDropbox = reportDropbox;
    }

    public void AddTopic(
        IFormattedTopicDefinition formattedTopicDefinition,
        Expression<Func<TMajor, object?>> memberExpression)
    {
        _reportDropbox.Dropoff(
            _parameters,
            _mod,
            Record,
            RecordTopic.Create(Record, formattedTopicDefinition, memberExpression));
    }
}
