using System.IO.Abstractions;
using Microsoft.Extensions.Logging;
using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Result;
using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.SkyrimAnalyzer
{
    [Analyzer]
    public class MissingAssetsAnalyzer : IMajorRecordAnalyzer<IArmorGetter>
    {
        public string Author => "erri120";
        public string Description => "Finds missing assets.";

        public const string MissingFemaleArmorModel = "Missing female armor model file";
        public const string MissingMaleArmorModel = "Missing male armor model file";

        private readonly ILogger<MissingAssetsAnalyzer> _logger;
        private readonly IFileSystem _fileSystem;

        public MissingAssetsAnalyzer(ILogger<MissingAssetsAnalyzer> logger, IFileSystem fileSystem)
        {
            _logger = logger;
            _fileSystem = fileSystem;
        }

        public AnalyzerResult AnalyzeRecord(IArmorGetter armorRecord)
        {
            var result = new AnalyzerResult();

            var femaleFile = armorRecord.WorldModel?.Female?.Model?.File;
            if (femaleFile != null)
            {
                if (!_fileSystem.File.Exists(femaleFile))
                    result.AddError(MissingFemaleArmorModel, armorRecord.FormKey);
            }

            var maleFile = armorRecord.WorldModel?.Male?.Model?.File;
            if (maleFile != null)
            {
                if (!_fileSystem.File.Exists(maleFile))
                    result.AddError(MissingMaleArmorModel, armorRecord.FormKey);
            }

            return result;
        }
    }
}
