using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Noggog;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Location;

public class MissingFieldsAnalyzer : IIsolatedRecordAnalyzer<ILocationGetter>
{
    public static readonly TopicDefinition NoParentLocation = MutagenTopicBuilder.DevelopmentTopic(
            "No Parent Location",
            Severity.Suggestion)
        .WithoutFormatting("Location has no parent location - this is likely a mistake - only top level locations should have no parent location");

    public IEnumerable<TopicDefinition> Topics { get; } = [NoParentLocation];

    private static readonly HashSet<IFormLinkGetter<ILocationGetter>> ValidTopLevelLocations =
    [
        FormKeys.SkyrimSE.Skyrim.Location.PersistAll,
        FormKeys.SkyrimSE.Skyrim.Location.HoldingCell,
        FormKeys.SkyrimSE.Skyrim.Location.VirtualLocation,
        FormKeys.SkyrimSE.Skyrim.Location.TamrielLocation,
        FormKeys.SkyrimSE.Skyrim.Location.SovngardeLocation,
        FormKeys.SkyrimSE.Dragonborn.Location.DLC2SolstheimLocation,
    ];

    public RecordAnalyzerResult? AnalyzeRecord(IsolatedRecordAnalyzerParams<ILocationGetter> param)
    {
        var location = param.Record;

        // Ignore some well known top level locations
        if (ValidTopLevelLocations.Contains(location))
        {
            return null;
        }

        if (location.ParentLocation.IsNull)
        {
            return new RecordAnalyzerResult(
                RecordTopic.Create(
                    location,
                    NoParentLocation.Format(),
                    x => x.ParentLocation));
        }

        return null;
    }
}
