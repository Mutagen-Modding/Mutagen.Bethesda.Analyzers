using System.IO.Abstractions;
using Microsoft.Extensions.Logging;
using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Result;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.SkyrimAnalyzer
{
    [Analyzer]
    public class MissingAssetsAnalyzer : IMajorRecordAnalyzer<IArmorGetter>, IMajorRecordAnalyzer<ITextureSetGetter>
    {
        public string Author => "erri120";
        public string Description => "Finds missing assets.";

        public const string MissingFemaleArmorModel = "Missing female armor model file";
        public const string MissingMaleArmorModel = "Missing male armor model file";

        public const string MissingDiffuseTexture = "Missing diffuse texture in TextureSet";
        public const string MissingNormalOrGlossTexture = "Missing normal or gloss texture in TextureSet";
        public const string MissingEnvironmentMaskOrSubsurfaceTintTexture = "Missing environment mask or subsurface tint texture in TextureSet";
        public const string MissingGlowOrDetailMap = "Missing glow or detail map in TextureSet";
        public const string MissingHeightTexture = "Missing height texture in TextureSet";
        public const string MissingEnvironmentTexture = "Missing environment texture in TextureSet";
        public const string MissingMultilayerTexture = "Missing multilayer texture in TextureSet";
        public const string MissingBacklightMaskOrSpecular = "Missing backlight mask or specular texture in TextureSet";

        private readonly ILogger<MissingAssetsAnalyzer> _logger;
        private readonly IFileSystem _fileSystem;

        public MissingAssetsAnalyzer(ILogger<MissingAssetsAnalyzer> logger, IFileSystem fileSystem)
        {
            _logger = logger;
            _fileSystem = fileSystem;
        }

        public AnalyzerResult AnalyzeRecord(IArmorGetter armor)
        {
            var result = new AnalyzerResult();

            var femaleFile = armor.WorldModel?.Female?.Model?.File;
            CheckForMissingAsset(femaleFile, armor.FormKey, result, MissingFemaleArmorModel);

            var maleFile = armor.WorldModel?.Male?.Model?.File;
            CheckForMissingAsset(maleFile, armor.FormKey, result, MissingMaleArmorModel);

            return result;
        }

        public AnalyzerResult AnalyzeRecord(ITextureSetGetter textureSet)
        {
            var result = new AnalyzerResult();

            CheckForMissingAsset(textureSet.Diffuse, textureSet.FormKey, result, MissingDiffuseTexture);
            CheckForMissingAsset(textureSet.NormalOrGloss, textureSet.FormKey, result, MissingNormalOrGlossTexture);
            CheckForMissingAsset(textureSet.EnvironmentMaskOrSubsurfaceTint, textureSet.FormKey, result, MissingEnvironmentMaskOrSubsurfaceTintTexture);
            CheckForMissingAsset(textureSet.GlowOrDetailMap, textureSet.FormKey, result, MissingGlowOrDetailMap);
            CheckForMissingAsset(textureSet.Height, textureSet.FormKey, result, MissingHeightTexture);
            CheckForMissingAsset(textureSet.Environment, textureSet.FormKey, result, MissingEnvironmentTexture);
            CheckForMissingAsset(textureSet.Multilayer, textureSet.FormKey, result, MissingMultilayerTexture);
            CheckForMissingAsset(textureSet.BacklightMaskOrSpecular, textureSet.FormKey, result, MissingBacklightMaskOrSpecular);

            return result;
        }

        private void CheckForMissingAsset(string? path, FormKey formKey, AnalyzerResult result, string error)
        {
            if (path == null) return;

            if (!_fileSystem.File.Exists(path))
                result.AddError(error, formKey);
        }
    }
}
