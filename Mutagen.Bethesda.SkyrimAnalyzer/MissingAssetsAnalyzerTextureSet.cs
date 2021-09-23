using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Errors;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Skyrim.Internals;

namespace Mutagen.Bethesda.SkyrimAnalyzer
{
    public partial class MissingAssetsAnalyzer : IMajorRecordAnalyzer<ITextureSetGetter>
    {
        public static readonly ErrorDefinition MissingTextureInTextureSet = new(
            "SOMEID",
            "Missing Texture in TextureSet",
            "TODO",
            Severity.Error);

        public MajorRecordAnalyzerResult AnalyzeRecord(ITextureSetGetter textureSet)
        {
            var result = new MajorRecordAnalyzerResult();

            CheckForMissingAsset(textureSet.Diffuse, result, () => RecordError.Create(MissingTextureInTextureSet, textureSet, RecordTypes.TXST, x => x.Diffuse!));
            CheckForMissingAsset(textureSet.NormalOrGloss, result, () => RecordError.Create(MissingTextureInTextureSet, textureSet, RecordTypes.TXST, x => x.NormalOrGloss!));
            CheckForMissingAsset(textureSet.EnvironmentMaskOrSubsurfaceTint, result, () => RecordError.Create(MissingTextureInTextureSet, textureSet, RecordTypes.TXST, x => x.EnvironmentMaskOrSubsurfaceTint!));
            CheckForMissingAsset(textureSet.GlowOrDetailMap, result, () => RecordError.Create(MissingTextureInTextureSet, textureSet, RecordTypes.TXST, x => x.GlowOrDetailMap!));
            CheckForMissingAsset(textureSet.Height, result, () => RecordError.Create(MissingTextureInTextureSet, textureSet, RecordTypes.TXST, x => x.Height!));
            CheckForMissingAsset(textureSet.Environment, result, () => RecordError.Create(MissingTextureInTextureSet, textureSet, RecordTypes.TXST, x => x.Environment!));
            CheckForMissingAsset(textureSet.Multilayer, result, () => RecordError.Create(MissingTextureInTextureSet, textureSet, RecordTypes.TXST, x => x.Multilayer!));
            CheckForMissingAsset(textureSet.BacklightMaskOrSpecular, result, () => RecordError.Create(MissingTextureInTextureSet, textureSet, RecordTypes.TXST, x => x.BacklightMaskOrSpecular!));

            return result;
        }
    }
}
