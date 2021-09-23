using System;
using System.IO.Abstractions;
using Microsoft.Extensions.Logging;
using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Errors;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.SkyrimAnalyzer
{
    [Analyzer]
    public partial class MissingAssetsAnalyzer : IMajorRecordAnalyzer<IArmorGetter>, IMajorRecordAnalyzer<ITextureSetGetter>,
        IMajorRecordAnalyzer<IWeaponGetter>, IMajorRecordAnalyzer<IStaticGetter>
    {
        public string Author => "erri120";
        public string Description => "Finds missing assets.";

        private readonly ILogger<MissingAssetsAnalyzer> _logger;
        private readonly IFileSystem _fileSystem;

        public MissingAssetsAnalyzer(ILogger<MissingAssetsAnalyzer> logger, IFileSystem fileSystem)
        {
            _logger = logger;
            _fileSystem = fileSystem;
        }

        private void CheckForMissingModelAsset<TMajorRecordGetter>(TMajorRecordGetter modeledGetter, MajorRecordAnalyzerResult result, ErrorDefinition errorDefinition, RecordType recordType)
            where TMajorRecordGetter : IMajorRecordGetter, IModeledGetter
        {
            CheckForMissingAsset(modeledGetter.Model?.File, result, () => RecordError.Create(
                errorDefinition, modeledGetter, recordType, x => x.Model!.File));
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
