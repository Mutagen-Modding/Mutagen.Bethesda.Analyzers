using System;
using System.IO.Abstractions;
using Microsoft.Extensions.Logging;
using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Errors;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Skyrim.Internals;

namespace Mutagen.Bethesda.SkyrimAnalyzer
{
    [Analyzer]
    public class MissingAssetsAnalyzer : IMajorRecordAnalyzer<IArmorGetter>, IMajorRecordAnalyzer<ITextureSetGetter>
    {
        public string Author => "erri120";
        public string Description => "Finds missing assets.";

        public static readonly ErrorDefinition MissingArmorModel = new(
            "SOMEID",
            "Missing Armor Model file",
            "TODO",
            Severity.Error);

        public static readonly ErrorDefinition MissingTextureInTextureSet = new(
            "SOMEID",
            "Missing Texture in TextureSet",
            "TODO",
            Severity.Error);

        private readonly ILogger<MissingAssetsAnalyzer> _logger;
        private readonly IFileSystem _fileSystem;

        public MissingAssetsAnalyzer(ILogger<MissingAssetsAnalyzer> logger, IFileSystem fileSystem)
        {
            _logger = logger;
            _fileSystem = fileSystem;
        }

        public MajorRecordAnalyzerResult AnalyzeRecord(IArmorGetter armor)
        {
            var result = new MajorRecordAnalyzerResult();

            var femaleFile = armor.WorldModel?.Female?.Model?.File;
            CheckForMissingAsset(femaleFile, result, () => RecordError.Create(
                MissingArmorModel,
                armor,
                RecordTypes.ARMO,
                x => x.WorldModel!.Female!.Model!.File));

            var maleFile = armor.WorldModel?.Male?.Model?.File;
            CheckForMissingAsset(maleFile, result, () => RecordError.Create(
                MissingArmorModel,
                armor,
                RecordTypes.ARMO,
                x => x.WorldModel!.Male!.Model!.File));

            return result;
        }

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

        private void CheckForMissingAsset(string? path, MajorRecordAnalyzerResult result, Func<RecordError> action)
        {
            if (path == null) return;

            if (!_fileSystem.File.Exists(path))
            {
                result.AddError(action());
            }
        }
    }
}
