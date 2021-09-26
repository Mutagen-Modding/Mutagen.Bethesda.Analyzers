using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.Analyzers.Skyrim
{
    public partial class MissingAssetsAnalyzer : IRecordAnalyzer<IArmorGetter>
    {
        public static readonly TopicDefinition<string, string?> MissingArmorModel = new(
            "SOMEID",
            "Missing Armor Model file",
            "Missing {0} model file at {1}",
            Severity.Error);

        public MajorRecordAnalyzerResult AnalyzeRecord(IRecordAnalyzerParams<IArmorGetter> param)
        {
            var result = new MajorRecordAnalyzerResult();

            var femaleFile = param.Record.WorldModel?.Female?.Model?.File;
            CheckForMissingAsset(femaleFile, result, () => RecordTopic.Create(param.Record,
                MissingArmorModel.Format("female", femaleFile),
                x => x.WorldModel!.Female!.Model!.File));

            var maleFile = param.Record.WorldModel?.Male?.Model?.File;
            CheckForMissingAsset(maleFile, result, () => RecordTopic.Create(param.Record,
                MissingArmorModel.Format("male", maleFile),
                x => x.WorldModel!.Male!.Model!.File));

            return result;
        }
    }
}
