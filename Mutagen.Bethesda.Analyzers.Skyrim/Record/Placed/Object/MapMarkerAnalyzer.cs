using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Placed.Object;

public class MapMarkerAnalyzer : IIsolatedRecordAnalyzer<IPlacedObjectGetter>
{
    public static readonly TopicDefinition NoMenuDisplayObject = MutagenTopicBuilder.DevelopmentTopic(
            "Not Persistent",
            Severity.Error)
        .WithoutFormatting("Map marker not persistent");

    public static readonly TopicDefinition NoLocRefType = MutagenTopicBuilder.DevelopmentTopic(
            "No Loc Ref Type",
            Severity.Suggestion)
        .WithoutFormatting("Map marker missing location reference type MapMarkerRefType");

    public static readonly TopicDefinition NoEditorID = MutagenTopicBuilder.DevelopmentTopic(
            "No EditorID",
            Severity.Silent)
        .WithoutFormatting("Map marker missing editor ID");

    public static readonly TopicDefinition NoLinkedReference = MutagenTopicBuilder.DevelopmentTopic(
            "No Linked Reference",
            Severity.Suggestion)
        .WithoutFormatting("Map marker missing linked reference for player spawn location");

    public IEnumerable<TopicDefinition> Topics { get; } = [NoMenuDisplayObject, NoLocRefType, NoEditorID, NoLinkedReference];

    public RecordAnalyzerResult AnalyzeRecord(IsolatedRecordAnalyzerParams<IPlacedObjectGetter> param)
    {
        var placedObject = param.Record;

        var result = new RecordAnalyzerResult();

        if (placedObject.Base.FormKey != FormKeys.SkyrimSE.Skyrim.Static.MapMarker.FormKey) return result;

        // Not Persistent
        if ((placedObject.SkyrimMajorRecordFlags & (SkyrimMajorRecord.SkyrimMajorRecordFlag)PlacedObject.DefaultMajorFlag.Persistent) == 0)
        {
            result.AddTopic(
                RecordTopic.Create(
                    placedObject,
                    NoMenuDisplayObject.Format(),
                    x => x.MajorRecordFlagsRaw));
        }

        // No Loc Ref Type
        if (placedObject.LocationRefTypes is null || placedObject.LocationRefTypes.All(link => link.FormKey != FormKeys.SkyrimSE.Skyrim.LocationReferenceType.MapMarkerRefType.FormKey))
        {
            result.AddTopic(
                RecordTopic.Create(
                    placedObject,
                    NoLocRefType.Format(),
                    x => x.LocationRefTypes));
        }

        // No EditorID
        if (placedObject.EditorID is null)
        {
            result.AddTopic(
                RecordTopic.Create(
                    placedObject,
                    NoEditorID.Format(),
                    x => x.EditorID));
        }

        // No Linked Reference
        if (placedObject.LinkedReferences.All(link => link.Reference.FormKey != FormKeys.SkyrimSE.Skyrim.Static.XMarkerHeading.FormKey))
        {
            result.AddTopic(
                RecordTopic.Create(
                    placedObject,
                    NoLinkedReference.Format(),
                    x => x.LinkedReferences));
        }

        return result;
    }
}
