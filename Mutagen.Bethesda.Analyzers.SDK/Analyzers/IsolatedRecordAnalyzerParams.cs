using Mutagen.Bethesda.Analyzers.SDK.Drops;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.SDK.Analyzers;

/// <summary>
/// Object containing all the parameters available for a <see cref="IIsolatedRecordAnalyzer{TMajor}"/>
/// </summary>
/// <typeparam name="TMajor">The type of record being analyzed</typeparam>
public readonly struct IsolatedRecordAnalyzerParams<TMajor>
    where TMajor : IMajorRecordGetter
{
    private readonly ModKey _mod;
    private readonly ReportContextParameters _parameters;
    private readonly IReportDropbox _reportDropbox;

    /// <summary>
    /// The record to be analyzed
    /// </summary>
    public readonly TMajor Record;

    internal IsolatedRecordAnalyzerParams(
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

    /// <summary>
    /// Reports a topic to the engine
    /// </summary>
    /// <param name="formattedTopicDefinition"></param>
    public void AddTopic(
        IFormattedTopicDefinition formattedTopicDefinition)
    {
        _reportDropbox.Dropoff(
            _parameters,
            _mod,
            Record,
            RecordTopic.Create(formattedTopicDefinition));
    }
}
