using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
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

    public void AnalyzeRecord(ContextualRecordAnalyzerParams<IPlacedObjectGetter> param)
    {
        var placedObject = param.Record;
        var scaleNullable = placedObject.Scale;
        if (scaleNullable is null) return;

        var scale = scaleNullable.Value;

        // Scale that is always allowed
        if (scale is >= 0.5f and <= 1.5f) return;

        // Allowed objects
        if (AllowedScaledObjects.Contains(placedObject.Base.FormKey)) return;

        var baseObject = placedObject.Base.TryResolve(param.LinkCache);
        var baseObjectEditorID = baseObject?.EditorID;
        if (baseObjectEditorID is null) return;

        // Allowed editor ids
        if (baseObjectEditorID.StartsWith("dwe", StringComparison.OrdinalIgnoreCase)) return;
        if (baseObjectEditorID.Contains("mine", StringComparison.OrdinalIgnoreCase)) return;
        if (baseObjectEditorID.Contains("cave", StringComparison.OrdinalIgnoreCase)) return;
        if (baseObjectEditorID.Contains("mountain", StringComparison.OrdinalIgnoreCase)) return;
        if (baseObjectEditorID.Contains("rock", StringComparison.OrdinalIgnoreCase)) return;
        if (baseObjectEditorID.Contains("water", StringComparison.OrdinalIgnoreCase)) return;
        if (baseObjectEditorID.Contains("fx", StringComparison.OrdinalIgnoreCase)) return;
        if (baseObjectEditorID.Contains("web", StringComparison.OrdinalIgnoreCase)) return;

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
                if (scale is >= 0.1f and <= 3f) return;

                break;
            case IFurnitureGetter:
                break;
            case IIngestibleGetter:
                break;
            case IMoveableStaticGetter:
                if (scale is >= 0.2f and <= 3f) return;

                break;
            case IStaticGetter:
                if (scale is >= 0.2f and <= 2.5f) return;

                break;
            case ITreeGetter:
                if (scale is >= 0.1f and <= 3f) return;

                break;
            // Ignored types
            case ILightGetter:
            case IIdleMarkerGetter:
            case ITextureSetGetter:
            case ISoundMarkerGetter:
            case ISpellGetter:
                return;
        }

        param.AddTopic(
            (scale < 1 ? ScaleTooSmall : ScaleTooLarge).Format(scale));
    }

    public IEnumerable<Func<IPlacedObjectGetter, object?>> FieldsOfInterest()
    {
        yield return x => x.Scale;
        yield return x => x.Base;
    }
}
