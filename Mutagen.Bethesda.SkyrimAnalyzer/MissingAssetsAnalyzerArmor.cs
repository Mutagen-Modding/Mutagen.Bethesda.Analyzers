using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Errors;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.SkyrimAnalyzer
{
    public partial class MissingAssetsAnalyzer : IMajorRecordAnalyzer<IArmorGetter>
    {
        public static readonly ErrorDefinition MissingArmorModel = new(
            "SOMEID",
            "Missing Armor Model file",
            "Missing {0} model file at {1}",
            Severity.Error);

        public MajorRecordAnalyzerResult AnalyzeRecord(IArmorGetter armor)
        {
            var result = new MajorRecordAnalyzerResult();

            var femaleFile = armor.WorldModel?.Female?.Model?.File;
            CheckForMissingAsset(femaleFile, result, () => RecordError.Create(armor,
                FormattedErrorDefinition.Create(MissingArmorModel, "female", femaleFile),
                x => x.WorldModel!.Female!.Model!.File));

            var maleFile = armor.WorldModel?.Male?.Model?.File;
            CheckForMissingAsset(maleFile, result, () => RecordError.Create(armor,
                FormattedErrorDefinition.Create(MissingArmorModel, "male", maleFile),
                x => x.WorldModel!.Male!.Model!.File));

            return result;
        }
    }
}
