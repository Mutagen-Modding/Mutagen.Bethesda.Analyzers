using Mutagen.Bethesda.Analyzers.SDK.Drops;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins.Binary.Headers;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.SDK.Analyzers;

/// <summary>
/// Object containing all the parameters available for a <see cref="IIsolatedRecordFrameAnalyzer{TMajor}"/>
/// </summary>
public readonly struct IsolatedRecordFrameAnalyzerParams<TMajor>
    where TMajor : IMajorRecordGetter
{
    private readonly IReportDropbox _reportDropbox;
    private readonly ReportContextParameters _parameters;

    public readonly MajorRecordFrame Frame;

    internal IsolatedRecordFrameAnalyzerParams(
        IReportDropbox reportDropbox,
        ReportContextParameters parameters,
        MajorRecordFrame frame)
    {
        _reportDropbox = reportDropbox;
        _parameters = parameters;
        Frame = frame;
    }

    /// <summary>
    /// Reports a topic to the engine
    /// </summary>
    public void AddTopic(
        IFormattedTopicDefinition formattedTopicDefinition)
    {
        _reportDropbox.Dropoff(
            _parameters,
            RecordTopic.Create(formattedTopicDefinition));
    }
}

/// <summary>
/// Object containing all the parameters available for a <see cref="IIsolatedRecordFrameAnalyzer"/>
/// </summary>
public readonly struct IsolatedRecordFrameAnalyzerParams
{
    private readonly IReportDropbox _reportDropbox;
    private readonly ReportContextParameters _parameters;

    public readonly MajorRecordFrame Frame;

    internal IsolatedRecordFrameAnalyzerParams(
        IReportDropbox reportDropbox,
        ReportContextParameters parameters,
        MajorRecordFrame frame)
    {
        _reportDropbox = reportDropbox;
        _parameters = parameters;
        Frame = frame;
    }

    /// <summary>
    /// Reports a topic to the engine
    /// </summary>
    public void AddTopic(
        IFormattedTopicDefinition formattedTopicDefinition)
    {
        _reportDropbox.Dropoff(
            _parameters,
            RecordTopic.Create(formattedTopicDefinition));
    }
}
