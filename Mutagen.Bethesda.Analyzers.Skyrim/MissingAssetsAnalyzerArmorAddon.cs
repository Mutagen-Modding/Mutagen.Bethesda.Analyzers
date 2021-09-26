using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.Analyzers.Skyrim
{
    public partial class MissingAssetsAnalyzer : IRecordAnalyzer<IArmorAddonGetter>
    {
        public static readonly TopicDefinition<string, string?> MissingArmorAddonWorldModel = new(
            "SOMEID",
            "Missing Armor Addon Model file",
            "Missing {0} Armor Addon Model file at {1}",
            Severity.Error);

        public static readonly TopicDefinition<string, string?> MissingArmorAddonFirstPersonModel = new(
            "SOMEID",
            "Missing Armor Addon 1st Person Model file",
            "Missing {0} 1st Person Armor Addon Model file at {1}",
            Severity.Error);

        public MajorRecordAnalyzerResult AnalyzeRecord(IRecordAnalyzerParams<IArmorAddonGetter> param)
        {
            var res = new MajorRecordAnalyzerResult();

            var femaleWorldModel = param.Record.WorldModel?.Female?.File;
            CheckForMissingAsset(femaleWorldModel, res, () => RecordTopic.Create(
                param.Record,
                MissingArmorAddonWorldModel.Format("female", femaleWorldModel),
                x => x.WorldModel!.Female!.File));

            var maleWorldModel = param.Record.WorldModel?.Male?.File;
            CheckForMissingAsset(maleWorldModel, res, () => RecordTopic.Create(
                param.Record,
                MissingArmorAddonWorldModel.Format("male", maleWorldModel),
                x => x.WorldModel!.Male!.File));

            var femaleFirstPersonModel = param.Record.FirstPersonModel?.Female?.File;
            CheckForMissingAsset(femaleFirstPersonModel, res, () => RecordTopic.Create(
                param.Record,
                MissingArmorAddonFirstPersonModel.Format("female", femaleFirstPersonModel),
                x => x.FirstPersonModel!.Female!.File));

            var maleFirstPersonModel = param.Record.FirstPersonModel?.Male?.File;
            CheckForMissingAsset(maleFirstPersonModel, res, () => RecordTopic.Create(
                param.Record,
                MissingArmorAddonFirstPersonModel.Format("male", maleFirstPersonModel),
                x => x.FirstPersonModel!.Male!.File));

            return res;
        }
    }
}
