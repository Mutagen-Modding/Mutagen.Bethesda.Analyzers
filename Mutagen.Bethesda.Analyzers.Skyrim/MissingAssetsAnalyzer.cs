using System.IO.Abstractions;
using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.Analyzers.Skyrim;

public partial class MissingAssetsAnalyzer
{
    public string Author => "erri120";
    public string Description => "Finds missing assets.";

    private const string MissingModelFileMessageFormat = "Missing Model file {0}";

    private readonly IFileSystem _fileSystem;

    public MissingAssetsAnalyzer(IFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
    }

    private void CheckForMissingModelAsset<TMajorRecordGetter>(
        IsolatedRecordAnalyzerParams<TMajorRecordGetter> param,
        TopicDefinition<string> topicDefinition)
        where TMajorRecordGetter : IMajorRecordGetter, IModeledGetter
    {
        var path = param.Record.Model?.File;
        if (path is null) return;
        if (FileExists(path)) return;

        param.AddTopic(topicDefinition.Format(path));
    }

    private bool FileExists(string path) => _fileSystem.File.Exists(path);
    private bool FileExistsIfNotNull(string? path) => path == null || _fileSystem.File.Exists(path);
}
