using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Errors;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.Analyzers.Skyrim
{
    public partial class MissingAssetsAnalyzer : IRecordAnalyzer<ITextureSetGetter>
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

        public MajorRecordAnalyzerResult AnalyzeRecord(IRecordAnalyzerParams<ITextureSetGetter> param)
        {
            var result = new MajorRecordAnalyzerResult();

            CheckForMissingAsset(param.Record.Diffuse, result, () => RecordError.Create(param.Record, FormattedErrorDefinition.Create(MissingTextureInTextureSet, TextureSetDiffuseName, param.Record.Diffuse), x => x.Diffuse!));
            CheckForMissingAsset(param.Record.NormalOrGloss, result, () => RecordError.Create(param.Record, FormattedErrorDefinition.Create(MissingTextureInTextureSet, TextureSetNormalOrGlossName, param.Record.NormalOrGloss), x => x.NormalOrGloss!));
            CheckForMissingAsset(param.Record.EnvironmentMaskOrSubsurfaceTint, result, () => RecordError.Create(param.Record, FormattedErrorDefinition.Create(MissingTextureInTextureSet, TextureSetEnvironmentMaskOrSubsurfaceTintName, param.Record.EnvironmentMaskOrSubsurfaceTint), x => x.EnvironmentMaskOrSubsurfaceTint!));
            CheckForMissingAsset(param.Record.GlowOrDetailMap, result, () => RecordError.Create(param.Record, FormattedErrorDefinition.Create(MissingTextureInTextureSet, TextureSetGlowOrDetailMapName, param.Record.GlowOrDetailMap), x => x.GlowOrDetailMap!));
            CheckForMissingAsset(param.Record.Height, result, () => RecordError.Create(param.Record, FormattedErrorDefinition.Create(MissingTextureInTextureSet, TextureSetHeightName, param.Record.Height), x => x.Height!));
            CheckForMissingAsset(param.Record.Environment, result, () => RecordError.Create(param.Record, FormattedErrorDefinition.Create(MissingTextureInTextureSet, TextureSetEnvironmentName, param.Record.Environment), x => x.Environment!));
            CheckForMissingAsset(param.Record.Multilayer, result, () => RecordError.Create(param.Record, FormattedErrorDefinition.Create(MissingTextureInTextureSet, TextureSetMultilayerName, param.Record.Multilayer), x => x.Multilayer!));
            CheckForMissingAsset(param.Record.BacklightMaskOrSpecular, result, () => RecordError.Create(param.Record, FormattedErrorDefinition.Create(MissingTextureInTextureSet, TextureSetBacklightMaskOrSpecularName, param.Record.BacklightMaskOrSpecular), x => x.BacklightMaskOrSpecular!));

            return result;
        }
    }
}
