using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Placed.Object;

public class ScaleAnalyzer : IContextualRecordAnalyzer<IPlacedObjectGetter>
{
    public static readonly TopicDefinition<float> ScaleTooSmall = MutagenTopicBuilder.DevelopmentTopic(
            "Scale Too Small",
            Severity.Warning)
        .WithFormatting<float>("The object with scale {0} is too small");

    public static readonly TopicDefinition<float> ScaleTooLarge = MutagenTopicBuilder.DevelopmentTopic(
            "Scale Too Large",
            Severity.Warning)
        .WithFormatting<float>("The object with scale {0} is too large");

    public IEnumerable<TopicDefinition> Topics { get; } = [ScaleTooSmall, ScaleTooLarge];

    public static readonly HashSet<FormKey> AllowedScaledObjects =
    [
        FormKeys.SkyrimSE.Skyrim.Static.BlackPlane01.FormKey,
        FormKeys.SkyrimSE.Skyrim.Door.AutoLoadDoor01.FormKey,
        FormKeys.SkyrimSE.Skyrim.Door.AutoLoadDoorMinUse01.FormKey,
        FormKeys.SkyrimSE.Skyrim.Door.AutoLoadDoorHiddenMinUse01.FormKey,
    ];

    public RecordAnalyzerResult? AnalyzeRecord(ContextualRecordAnalyzerParams<IPlacedObjectGetter> param)
    {
        var placedObject = param.Record;
        var scaleNullable = placedObject.Scale;
        if (scaleNullable is null) return null;

        var scale = scaleNullable.Value;

        // Scale that is always allowed
        if (scale is >= 0.5f and <= 1.5f) return null;

        // Allowed objects
        if (AllowedScaledObjects.Contains(placedObject.Base.FormKey)) return null;

        var baseObject = placedObject.Base.TryResolve(param.LinkCache);
        var baseObjectEditorID = baseObject?.EditorID;
        if (baseObjectEditorID is null) return null;

        // Allowed editor ids
        if (baseObjectEditorID.StartsWith("dwe", StringComparison.OrdinalIgnoreCase)) return null;
        if (baseObjectEditorID.Contains("mine", StringComparison.OrdinalIgnoreCase)) return null;
        if (baseObjectEditorID.Contains("cave", StringComparison.OrdinalIgnoreCase)) return null;
        if (baseObjectEditorID.Contains("mountain", StringComparison.OrdinalIgnoreCase)) return null;
        if (baseObjectEditorID.Contains("rock", StringComparison.OrdinalIgnoreCase)) return null;
        if (baseObjectEditorID.Contains("water", StringComparison.OrdinalIgnoreCase)) return null;
        if (baseObjectEditorID.Contains("fx", StringComparison.OrdinalIgnoreCase)) return null;
        if (baseObjectEditorID.Contains("web", StringComparison.OrdinalIgnoreCase)) return null;

        // Specific type filter
        switch (baseObject)
        {
            case IActivatorGetter:
                break;
            case IContainerGetter:
                break;
            case IDoorGetter:
                break;
            case IFloraGetter:
                if (scale is >= 0.1f and <= 3f) return null;

                break;
            case IFurnitureGetter:
                break;
            case IIngestibleGetter:
                break;
            case IMoveableStaticGetter:
                if (scale is >= 0.2f and <= 3f) return null;

                break;
            case IStaticGetter:
                if (scale is >= 0.2f and <= 2.5f) return null;

                break;
            case ITreeGetter:
                if (scale is >= 0.1f and <= 3f) return null;

                break;
            // Ignored types
            case ILightGetter:
            case IIdleMarkerGetter:
            case ITextureSetGetter:
            case ISoundMarkerGetter:
            case ISpellGetter:
                return null;
        }

        return new RecordAnalyzerResult(
            RecordTopic.Create(
                placedObject,
                ScaleTooSmall.Format(scale),
                x => x.Scale));
    }
}
