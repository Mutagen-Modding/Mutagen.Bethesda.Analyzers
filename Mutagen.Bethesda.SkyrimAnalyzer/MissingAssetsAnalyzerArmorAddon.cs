using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Errors;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.SkyrimAnalyzer
{
    public partial class MissingAssetsAnalyzer : IMajorRecordAnalyzer<IArmorAddonGetter>
    {
        public static readonly ErrorDefinition MissingArmorAddonWorldModel = new(
            "SOMEID",
            "Missing Armor Addon Model file",
            "Missing {0} Armor Addon Model file at {1}",
            Severity.Error);

        public static readonly ErrorDefinition MissingArmorAddonFirstPersonModel = new(
            "SOMEID",
            "Missing Armor Addon 1st Person Model file",
            "Missing {0} 1st Person Armor Addon Model file at {1}",
            Severity.Error);

        public MajorRecordAnalyzerResult AnalyzeRecord(IArmorAddonGetter armorAddon)
        {
            var res = new MajorRecordAnalyzerResult();

            var femaleWorldModel = armorAddon.WorldModel?.Female?.File;
            CheckForMissingAsset(femaleWorldModel, res, () => RecordError.Create(
                armorAddon,
                FormattedErrorDefinition.Create(MissingArmorAddonWorldModel, "female", femaleWorldModel),
                x => x.WorldModel!.Female!.File));

            var maleWorldModel = armorAddon.WorldModel?.Male?.File;
            CheckForMissingAsset(maleWorldModel, res, () => RecordError.Create(
                armorAddon,
                FormattedErrorDefinition.Create(MissingArmorAddonWorldModel, "male", maleWorldModel),
                x => x.WorldModel!.Male!.File));

            var femaleFirstPersonModel = armorAddon.FirstPersonModel?.Female?.File;
            CheckForMissingAsset(femaleFirstPersonModel, res, () => RecordError.Create(
                armorAddon,
                FormattedErrorDefinition.Create(MissingArmorAddonFirstPersonModel, "female", femaleFirstPersonModel),
                x => x.FirstPersonModel!.Female!.File));

            var maleFirstPersonModel = armorAddon.FirstPersonModel?.Male?.File;
            CheckForMissingAsset(maleFirstPersonModel, res, () => RecordError.Create(
                armorAddon,
                FormattedErrorDefinition.Create(MissingArmorAddonFirstPersonModel, "male", maleFirstPersonModel),
                x => x.FirstPersonModel!.Male!.File));

            return res;
        }
    }
}
