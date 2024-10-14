using System.Runtime.CompilerServices;
using Mutagen.Bethesda.Analyzers.SDK.Drops;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.SDK.Analyzers;

/// <summary>
/// Object containing all the parameters available for a <see cref="IIsolatedRecordsAnalyzer{TDriver, TLookup1}"/>
/// </summary>
/// <typeparam name="TDriver">The type of record driving the analysis</typeparam>
/// <typeparam name="TLookup1">A type of record allowed to be resolved by the link cache</typeparam>
public readonly record struct IsolatedRecordsAnalyzerParams<TDriver, TLookup1>
    where TDriver : class, IMajorRecordGetter
    where TLookup1 : class, IMajorRecordGetter
{
    public Type? AnalyzerType { get; init; }
    public readonly IRecordLookup<TLookup1> Lookup1;
    private readonly ModKey _mod;
    private readonly ReportContextParameters _parameters;
    private readonly IReportDropbox _reportDropbox;

    /// <summary>
    /// The record to be analyzed
    /// </summary>
    public readonly TDriver Record;

    internal IsolatedRecordsAnalyzerParams(
        IRecordLookup<TLookup1> lookup1,
        ModKey mod,
        TDriver record,
        ReportContextParameters parameters,
        IReportDropbox reportDropbox)
    {
        Lookup1 = lookup1;
        _mod = mod;
        Record = record;
        _parameters = parameters;
        _reportDropbox = reportDropbox;
    }

    /// <summary>
    /// Reports a topic to the engine
    /// </summary>
    public void AddTopicWithNoLookups(
        IFormattedTopicDefinition formattedTopicDefinition,
        params (string Name, object Value)[] metaData)
    {
        _reportDropbox.Dropoff(
            _parameters,
            _mod,
            Record,
            Topic.Create(formattedTopicDefinition, AnalyzerType, metaData));
    }

    /// <summary>
    /// Reports a topic to the engine
    /// </summary>
    public void AddTopic(
        IFormattedTopicDefinition formattedTopicDefinition,
        TLookup1 record,
        params (string Name, object Value)[] metaData)
    {
        // ToDo
        // Dropoff 2nd record as well somehow

        _reportDropbox.Dropoff(
            _parameters,
            _mod,
            Record,
            Topic.Create(formattedTopicDefinition, AnalyzerType, metaData));
    }

    /// <summary>
    /// Reports a topic to the engine
    /// </summary>
    public void AddTopic(
        IFormattedTopicDefinition formattedTopicDefinition,
        IReadOnlyCollection<TLookup1> records,
        params (string Name, object Value)[] metaData)
    {
        // ToDo
        // Dropoff 2nd record as well somehow

        _reportDropbox.Dropoff(
            _parameters,
            _mod,
            Record,
            Topic.Create(formattedTopicDefinition, AnalyzerType, metaData));
    }

    /// <summary>
    /// Reports a topic to the engine
    /// </summary>
    public void AddTopic(
        IFormattedTopicDefinition formattedTopicDefinition,
        IFormLinkGetter<TLookup1> record,
        params (string Name, object Value)[] metaData)
    {
        // ToDo
        // Dropoff 2nd record as well somehow

        _reportDropbox.Dropoff(
            _parameters,
            _mod,
            Record,
            Topic.Create(formattedTopicDefinition, AnalyzerType, metaData));
    }
}

/// <summary>
/// Object containing all the parameters available for a <see cref="IIsolatedRecordsAnalyzer{TDriver, TLookup1, TLookup2}"/>
/// </summary>
/// <typeparam name="TDriver">The type of record driving the analysis</typeparam>
/// <typeparam name="TLookup1">A type of record allowed to be resolved by the link cache</typeparam>
/// <typeparam name="TLookup2">A type of record allowed to be resolved by the link cache</typeparam>
public readonly record struct IsolatedRecordsAnalyzerParams<TDriver, TLookup1, TLookup2>
    where TDriver : class, IMajorRecordGetter
    where TLookup1 : class, IMajorRecordGetter
    where TLookup2 : class, IMajorRecordGetter
{
    public Type? AnalyzerType { get; init; }
    public readonly IRecordLookup<TLookup1> Lookup1;
    public readonly IRecordLookup<TLookup2> Lookup2;
    private readonly ModKey _mod;
    private readonly ReportContextParameters _parameters;
    private readonly IReportDropbox _reportDropbox;

    /// <summary>
    /// The record to be analyzed
    /// </summary>
    public readonly TDriver Record;

    internal IsolatedRecordsAnalyzerParams(
        ModKey mod,
        TDriver record,
        ReportContextParameters parameters,
        IReportDropbox reportDropbox,
        IRecordLookup<TLookup1> lookup1,
        IRecordLookup<TLookup2> lookup2)
    {
        _mod = mod;
        Record = record;
        _parameters = parameters;
        _reportDropbox = reportDropbox;
        Lookup1 = lookup1;
        Lookup2 = lookup2;
    }

    /// <summary>
    /// Reports a topic to the engine
    /// </summary>
    public void AddTopicWithNoLookups(
        IFormattedTopicDefinition formattedTopicDefinition,
        params (string Name, object Value)[] metaData)
    {
        _reportDropbox.Dropoff(
            _parameters,
            _mod,
            Record,
            Topic.Create(formattedTopicDefinition, AnalyzerType, metaData));
    }

    /// <summary>
    /// Reports a topic to the engine
    /// </summary>
    public void AddTopic(
        IFormattedTopicDefinition formattedTopicDefinition,
        TLookup1 record,
        params (string Name, object Value)[] metaData)
    {
        // ToDo
        // Dropoff 2nd record as well somehow

        _reportDropbox.Dropoff(
            _parameters,
            _mod,
            Record,
            Topic.Create(formattedTopicDefinition, AnalyzerType, metaData));
    }

    /// <summary>
    /// Reports a topic to the engine
    /// </summary>
    public void AddTopic(
        IFormattedTopicDefinition formattedTopicDefinition,
        IReadOnlyCollection<TLookup1> records,
        params (string Name, object Value)[] metaData)
    {
        // ToDo
        // Dropoff 2nd record as well somehow

        _reportDropbox.Dropoff(
            _parameters,
            _mod,
            Record,
            Topic.Create(formattedTopicDefinition, AnalyzerType, metaData));
    }

    /// <summary>
    /// Reports a topic to the engine
    /// </summary>
    public void AddTopic(
        IFormattedTopicDefinition formattedTopicDefinition,
        IFormLinkGetter<TLookup1> record,
        params (string Name, object Value)[] metaData)
    {
        // ToDo
        // Dropoff 2nd record as well somehow

        _reportDropbox.Dropoff(
            _parameters,
            _mod,
            Record,
            Topic.Create(formattedTopicDefinition, AnalyzerType, metaData));
    }
}
