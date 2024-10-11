using System.IO.Abstractions;
using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.Analyzers.Skyrim.Util;

public class MissingAssetsAnalyzerUtil
{
    private readonly IFileSystem _fileSystem;

    public MissingAssetsAnalyzerUtil(IFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
    }

    public void CheckForMissingModelAsset<TMajorRecordGetter>(
        IsolatedRecordAnalyzerParams<TMajorRecordGetter> param,
        TopicDefinition<string> topicDefinition)
        where TMajorRecordGetter : IMajorRecordGetter, IModeledGetter
    {
        var path = param.Record.Model?.File;
        if (path is null) return;
        if (FileExists(path)) return;

        param.AddTopic(topicDefinition.Format(path));
    }

    public bool FileExists(string path) => _fileSystem.File.Exists(path);
    public bool FileExistsIfNotNull(string? path) => path == null || _fileSystem.File.Exists(path);
}
