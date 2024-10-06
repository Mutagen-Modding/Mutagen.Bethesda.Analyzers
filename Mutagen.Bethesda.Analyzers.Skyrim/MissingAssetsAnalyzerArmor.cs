using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.Analyzers.Skyrim;

public partial class MissingAssetsAnalyzer : IIsolatedRecordAnalyzer<IArmorGetter>
{
    public static readonly TopicDefinition<string, string?> MissingArmorModel = MutagenTopicBuilder.FromDiscussion(
            82,
            "Missing Armor Model file",
            Severity.Error)
        .WithFormatting<string, string?>("Missing {0} model file at {1}");

    public void AnalyzeRecord(IsolatedRecordAnalyzerParams<IArmorGetter> param)
    {
        var femaleFile = param.Record.WorldModel?.Female?.Model?.File;
        if (!FileExistsIfNotNull(femaleFile))
        {
            param.AddTopic(
                MissingArmorModel.Format("female", femaleFile));
        }
        var maleFile = param.Record.WorldModel?.Male?.Model?.File;
        if (!FileExistsIfNotNull(maleFile))
        {
            param.AddTopic(
                MissingArmorModel.Format("male", maleFile));
        }
    }

    IEnumerable<Func<IArmorGetter, object?>> IIsolatedRecordAnalyzer<IArmorGetter>.FieldsOfInterest()
    {
        yield return x => x.WorldModel;
    }
}
