using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.Analyzers.Skyrim;

public partial class MissingAssetsAnalyzer : IIsolatedRecordAnalyzer<ITextureSetGetter>
{
    public static readonly TopicDefinition<string, string?> MissingTextureInTextureSet = MutagenTopicBuilder.FromDiscussion(
            91,
            "Missing Texture in TextureSet",
            Severity.Error)
        .WithFormatting<string, string?>("Missing texture {0} at {1}");

    private const string TextureSetDiffuseName = nameof(ITextureSet.Diffuse);
    private const string TextureSetNormalOrGlossName = "Normal/Gloss";
    private const string TextureSetEnvironmentMaskOrSubsurfaceTintName = "Environment Maks/Subsurface Tint";
    private const string TextureSetGlowOrDetailMapName = "Glow/Detail Map";
    private const string TextureSetHeightName = nameof(ITextureSet.Height);
    private const string TextureSetEnvironmentName = nameof(ITextureSet.Environment);
    private const string TextureSetMultilayerName = nameof(ITextureSet.Multilayer);
    private const string TextureSetBacklightMaskOrSpecularName = "Backlight Mask/Specular";

    public void AnalyzeRecord(IsolatedRecordAnalyzerParams<ITextureSetGetter> param)
    {
        if (!FileExistsIfNotNull(param.Record.Diffuse))
        {
            param.AddTopic(
                MissingTextureInTextureSet.Format(TextureSetDiffuseName, param.Record.Diffuse),
                x => x.Diffuse!);
        }

        if (!FileExistsIfNotNull(param.Record.NormalOrGloss))
        {
            param.AddTopic(
                MissingTextureInTextureSet.Format(TextureSetNormalOrGlossName, param.Record.NormalOrGloss),
                x => x.NormalOrGloss!);
        }

        if (!FileExistsIfNotNull(param.Record.EnvironmentMaskOrSubsurfaceTint))
        {
            param.AddTopic(
                MissingTextureInTextureSet.Format(TextureSetEnvironmentMaskOrSubsurfaceTintName, param.Record.EnvironmentMaskOrSubsurfaceTint),
                x => x.EnvironmentMaskOrSubsurfaceTint!);
        }

        if (!FileExistsIfNotNull(param.Record.GlowOrDetailMap))
        {
            param.AddTopic(
                MissingTextureInTextureSet.Format(TextureSetGlowOrDetailMapName, param.Record.GlowOrDetailMap),
                x => x.GlowOrDetailMap!);
        }

        if (!FileExistsIfNotNull(param.Record.Height))
        {
            param.AddTopic(
                MissingTextureInTextureSet.Format(TextureSetHeightName, param.Record.Height),
                x => x.Height!);
        }

        if (!FileExistsIfNotNull(param.Record.Environment))
        {
            param.AddTopic(
                MissingTextureInTextureSet.Format(TextureSetEnvironmentName, param.Record.Environment),
                x => x.Environment!);
        }

        if (!FileExistsIfNotNull(param.Record.Multilayer))
        {
            param.AddTopic(
                MissingTextureInTextureSet.Format(TextureSetMultilayerName, param.Record.Multilayer),
                x => x.Multilayer!);
        }

        if (!FileExistsIfNotNull(param.Record.BacklightMaskOrSpecular))
        {
            param.AddTopic(
                MissingTextureInTextureSet.Format(TextureSetBacklightMaskOrSpecularName, param.Record.BacklightMaskOrSpecular),
                x => x.BacklightMaskOrSpecular!);
        }
    }
}
