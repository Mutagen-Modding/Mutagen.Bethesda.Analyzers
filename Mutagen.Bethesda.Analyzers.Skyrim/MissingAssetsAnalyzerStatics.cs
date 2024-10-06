using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.Analyzers.Skyrim;

public partial class MissingAssetsAnalyzer : IIsolatedRecordAnalyzer<IStaticGetter>
{
    public static readonly TopicDefinition<string> MissingStaticModel = MutagenTopicBuilder.FromDiscussion(
            90,
            "Missing Static Model file",
            Severity.Error)
        .WithFormatting<string>(MissingModelFileMessageFormat);

    public void AnalyzeRecord(IsolatedRecordAnalyzerParams<IStaticGetter> param)
    {
        CheckForMissingModelAsset(param, MissingStaticModel);
    }

    IEnumerable<Func<IStaticGetter, object?>> IIsolatedRecordAnalyzer<IStaticGetter>.FieldsOfInterest()
    {
        yield return x => x.Model!.File;
    }
}
