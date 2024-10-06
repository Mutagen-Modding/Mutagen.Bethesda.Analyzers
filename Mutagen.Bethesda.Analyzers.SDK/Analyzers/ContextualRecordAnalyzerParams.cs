using Mutagen.Bethesda.Analyzers.SDK.Drops;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Cache;
using Mutagen.Bethesda.Plugins.Order;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.SDK.Analyzers;

public readonly struct ContextualRecordAnalyzerParams<TMajor>
    where TMajor : IMajorRecordGetter
{
    public readonly ILinkCache LinkCache;
    public readonly ILoadOrderGetter<IModListingGetter<IModGetter>> LoadOrder;
    public readonly TMajor Record;
    private readonly IReportDropbox _reportDropbox;
    private readonly ReportContextParameters _parameters;

    internal ContextualRecordAnalyzerParams(
        ILinkCache linkCache,
        ILoadOrderGetter<IModListingGetter<IModGetter>> loadOrder,
        TMajor record,
        IReportDropbox reportDropbox)
    {
        LinkCache = linkCache;
        LoadOrder = loadOrder;
        Record = record;
        _reportDropbox = reportDropbox;
        _parameters = new ReportContextParameters(linkCache);
    }

    public void AddTopic(
        IFormattedTopicDefinition formattedTopicDefinition)
    {
        _reportDropbox.Dropoff(
            _parameters,
            RecordTopic.Create(formattedTopicDefinition));
    }

    public void AddTopic(
        ModKey mod,
        TMajor record,
        IFormattedTopicDefinition formattedTopicDefinition)
    {
        _reportDropbox.Dropoff(
            _parameters,
            RecordTopic.Create(formattedTopicDefinition));
    }
}
