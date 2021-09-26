using System;
using System.IO.Abstractions;
using Microsoft.Extensions.Logging;
using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Errors;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.Analyzers.Skyrim
{
    [Analyzer]
    public partial class MissingAssetsAnalyzer
    {
        public string Author => "erri120";
        public string Description => "Finds missing assets.";

        private const string MissingModelFileMessageFormat = "Missing Model file {0}";

        private readonly ILogger<MissingAssetsAnalyzer> _logger;
        private readonly IFileSystem _fileSystem;

        public MissingAssetsAnalyzer(ILogger<MissingAssetsAnalyzer> logger, IFileSystem fileSystem)
        {
            _logger = logger;
            _fileSystem = fileSystem;
        }

        private void CheckForMissingModelAsset<TMajorRecordGetter>(
            TMajorRecordGetter modeledGetter,
            MajorRecordAnalyzerResult result,
            ErrorDefinition errorDefinition)
            where TMajorRecordGetter : IMajorRecordGetter, IModeledGetter
        {
            var path = modeledGetter.Model?.File;
            if (path == null) return;
            if (FileExists(path)) return;

            var formattedErrorDefinition = FormattedErrorDefinition.Create(errorDefinition, path);
            var error = RecordError.Create(
                modeledGetter,
                formattedErrorDefinition,
                x => x.Model!.File);

            result.AddError(error);
        }

        private void CheckForMissingAsset(string? path, MajorRecordAnalyzerResult result, Func<RecordError> func)
        {
            if (path == null) return;
            if (FileExists(path)) return;

            var error = func();
            result.AddError(error);
        }

        private bool FileExists(string path) => _fileSystem.File.Exists(path);
    }
}
