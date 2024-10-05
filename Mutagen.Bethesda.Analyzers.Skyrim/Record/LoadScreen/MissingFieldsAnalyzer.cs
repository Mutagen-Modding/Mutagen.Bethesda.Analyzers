using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
using Noggog;

namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.LoadScreen;

public class MissingFieldsAnalyzer : IIsolatedRecordAnalyzer<ILoadScreenGetter>
{
    public static readonly TopicDefinition NoDescription = MutagenTopicBuilder.DevelopmentTopic(
            "No Description",
            Severity.Suggestion)
        .WithoutFormatting("LoadScreen has no description");

    public static readonly TopicDefinition No3DModel = MutagenTopicBuilder.DevelopmentTopic(
            "No 3D Model",
            Severity.Suggestion)
        .WithoutFormatting("LoadScreen has no 3D model");

    public IEnumerable<TopicDefinition> Topics { get; } = [NoDescription, No3DModel];

    public void AnalyzeRecord(IsolatedRecordAnalyzerParams<ILoadScreenGetter> param)
    {
        var loadScreen = param.Record;

        if (loadScreen.Description.String.IsNullOrWhitespace())
        {
            param.AddTopic(
                NoDescription.Format(),
                x => x.Description);
        }

        if (loadScreen.LoadingScreenNif.IsNull)
        {
            param.AddTopic(
                No3DModel.Format(),
                x => x.LoadingScreenNif);
        }
    }
}
