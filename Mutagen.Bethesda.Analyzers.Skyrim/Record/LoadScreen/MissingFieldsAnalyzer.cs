using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
using Noggog;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.LoadScreen;

public class MissingFieldsAnalyzer : IIsolatedRecordAnalyzer<ILoadScreenGetter>
{
    public static readonly TopicDefinition NoDescription = MutagenTopicBuilder.FromDiscussion(
            0,
            "No Description",
            Severity.Suggestion)
        .WithoutFormatting("LoadScreen has no description");

    public static readonly TopicDefinition No3DModel = MutagenTopicBuilder.FromDiscussion(
            0,
            "No 3D Model",
            Severity.Suggestion)
        .WithoutFormatting("LoadScreen has no 3D model");

    public IEnumerable<TopicDefinition> Topics { get; } = [NoDescription, No3DModel];

    public RecordAnalyzerResult AnalyzeRecord(IsolatedRecordAnalyzerParams<ILoadScreenGetter> param)
    {
        var loadScreen = param.Record;

        var result = new RecordAnalyzerResult();

        if (loadScreen.Description.String.IsNullOrWhitespace())
        {
            result.AddTopic(RecordTopic.Create(
                loadScreen,
                NoDescription.Format(),
                x => x.Description));
        }

        if (loadScreen.LoadingScreenNif.IsNull)
        {
            result.AddTopic(RecordTopic.Create(
                loadScreen,
                No3DModel.Format(),
                x => x.LoadingScreenNif));
        }

        return result;
    }
}
