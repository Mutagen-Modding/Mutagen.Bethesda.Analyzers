using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Noggog;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Location;

public class MissingFieldsAnalyzer : IIsolatedRecordAnalyzer<ILocationGetter>
{
    public static readonly TopicDefinition NoParentLocation = MutagenTopicBuilder.FromDiscussion(
            0,
            "No Parent Location",
            Severity.Suggestion)
        .WithoutFormatting("Location has no parent location - this is likely a mistake - only top level locations should have no parent location");

    public IEnumerable<TopicDefinition> Topics { get; } = [NoParentLocation];

    private static readonly HashSet<FormKey> ValidTopLevelLocations =
    [
        FormKeys.SkyrimSE.Skyrim.Location.PersistAll.FormKey,
        FormKeys.SkyrimSE.Skyrim.Location.HoldingCell.FormKey,
        FormKeys.SkyrimSE.Skyrim.Location.VirtualLocation.FormKey,
        FormKeys.SkyrimSE.Skyrim.Location.TamrielLocation.FormKey,
        FormKeys.SkyrimSE.Skyrim.Location.SovngardeLocation.FormKey,
        FormKeys.SkyrimSE.Dragonborn.Location.DLC2SolstheimLocation.FormKey,
    ];

    public RecordAnalyzerResult AnalyzeRecord(IsolatedRecordAnalyzerParams<ILocationGetter> param)
    {
        var location = param.Record;

        var result = new RecordAnalyzerResult();

        if (location.ParentLocation.IsNull)
        {
            result.AddTopic(RecordTopic.Create(
                location,
                NoParentLocation.Format(),
                x => x.ParentLocation));
        }

        return result;
    }
}
