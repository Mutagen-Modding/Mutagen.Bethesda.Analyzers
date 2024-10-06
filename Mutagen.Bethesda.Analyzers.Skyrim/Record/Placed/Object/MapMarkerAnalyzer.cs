using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
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

    public void AnalyzeRecord(IsolatedRecordAnalyzerParams<IPlacedObjectGetter> param)
    {
        var placedObject = param.Record;

        if (placedObject.Base.FormKey != FormKeys.SkyrimSE.Skyrim.Static.MapMarker.FormKey) return;

        // Not Persistent
        if ((placedObject.SkyrimMajorRecordFlags & (SkyrimMajorRecord.SkyrimMajorRecordFlag)PlacedObject.DefaultMajorFlag.Persistent) == 0)
        {
            param.AddTopic(
                NoMenuDisplayObject.Format());
        }

        // No Loc Ref Type
        if (placedObject.LocationRefTypes is null || placedObject.LocationRefTypes.All(link => link.FormKey != FormKeys.SkyrimSE.Skyrim.LocationReferenceType.MapMarkerRefType.FormKey))
        {
            param.AddTopic(
                NoLocRefType.Format());
        }

        // No EditorID
        if (placedObject.EditorID is null)
        {
            param.AddTopic(
                NoEditorID.Format());
        }

        // No Linked Reference
        if (placedObject.LinkedReferences.All(link => link.Reference.FormKey != FormKeys.SkyrimSE.Skyrim.Static.XMarkerHeading.FormKey))
        {
            param.AddTopic(
                NoLinkedReference.Format());
        }
    }

    public IEnumerable<Func<IPlacedObjectGetter, object?>> FieldsOfInterest()
    {
        yield return x => x.EditorID;
        yield return x => x.LinkedReferences;
        yield return x => x.LocationRefTypes;
        yield return x => x.SkyrimMajorRecordFlags;
    }
}
