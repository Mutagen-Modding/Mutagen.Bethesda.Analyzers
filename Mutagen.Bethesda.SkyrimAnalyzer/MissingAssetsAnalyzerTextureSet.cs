using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Errors;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.SkyrimAnalyzer
{
    public partial class MissingAssetsAnalyzer : IMajorRecordAnalyzer<ITextureSetGetter>
    {
        public static readonly ErrorDefinition MissingTextureInTextureSet = new(
            "SOMEID",
            "Missing Texture in TextureSet",
            "Missing texture {0} at {1}",
            Severity.Error);

        private const string TextureSetDiffuseName = nameof(ITextureSet.Diffuse);
        private const string TextureSetNormalOrGlossName = "Normal/Gloss";
        private const string TextureSetEnvironmentMaskOrSubsurfaceTintName = "Environment Maks/Subsurface Tint";
        private const string TextureSetGlowOrDetailMapName = "Glow/Detail Map";
        private const string TextureSetHeightName = nameof(ITextureSet.Height);
        private const string TextureSetEnvironmentName = nameof(ITextureSet.Environment);
        private const string TextureSetMultilayerName = nameof(ITextureSet.Multilayer);
        private const string TextureSetBacklightMaskOrSpecularName = "Backlight Mask/Specular";

        public MajorRecordAnalyzerResult AnalyzeRecord(ITextureSetGetter textureSet)
        {
            var result = new MajorRecordAnalyzerResult();

            CheckForMissingAsset(textureSet.Diffuse, result, () => RecordError.Create(textureSet, FormattedErrorDefinition.Create(MissingTextureInTextureSet, TextureSetDiffuseName, textureSet.Diffuse), x => x.Diffuse!));
            CheckForMissingAsset(textureSet.NormalOrGloss, result, () => RecordError.Create(textureSet, FormattedErrorDefinition.Create(MissingTextureInTextureSet, TextureSetNormalOrGlossName, textureSet.NormalOrGloss), x => x.NormalOrGloss!));
            CheckForMissingAsset(textureSet.EnvironmentMaskOrSubsurfaceTint, result, () => RecordError.Create(textureSet, FormattedErrorDefinition.Create(MissingTextureInTextureSet, TextureSetEnvironmentMaskOrSubsurfaceTintName, textureSet.EnvironmentMaskOrSubsurfaceTint), x => x.EnvironmentMaskOrSubsurfaceTint!));
            CheckForMissingAsset(textureSet.GlowOrDetailMap, result, () => RecordError.Create(textureSet, FormattedErrorDefinition.Create(MissingTextureInTextureSet, TextureSetGlowOrDetailMapName, textureSet.GlowOrDetailMap), x => x.GlowOrDetailMap!));
            CheckForMissingAsset(textureSet.Height, result, () => RecordError.Create(textureSet, FormattedErrorDefinition.Create(MissingTextureInTextureSet, TextureSetHeightName, textureSet.Height), x => x.Height!));
            CheckForMissingAsset(textureSet.Environment, result, () => RecordError.Create(textureSet, FormattedErrorDefinition.Create(MissingTextureInTextureSet, TextureSetEnvironmentName, textureSet.Environment), x => x.Environment!));
            CheckForMissingAsset(textureSet.Multilayer, result, () => RecordError.Create(textureSet, FormattedErrorDefinition.Create(MissingTextureInTextureSet, TextureSetMultilayerName, textureSet.Multilayer), x => x.Multilayer!));
            CheckForMissingAsset(textureSet.BacklightMaskOrSpecular, result, () => RecordError.Create(textureSet, FormattedErrorDefinition.Create(MissingTextureInTextureSet, TextureSetBacklightMaskOrSpecularName, textureSet.BacklightMaskOrSpecular), x => x.BacklightMaskOrSpecular!));

            return result;
        }
    }
}
