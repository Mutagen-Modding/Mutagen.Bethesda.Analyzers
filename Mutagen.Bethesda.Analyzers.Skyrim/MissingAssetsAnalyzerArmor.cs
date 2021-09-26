using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Errors;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.Analyzers.Skyrim
{
    public partial class MissingAssetsAnalyzer : IRecordAnalyzer<IArmorGetter>
    {
        public static readonly ErrorDefinition MissingArmorModel = new(
            "SOMEID",
            "Missing Armor Model file",
            "Missing {0} model file at {1}",
            Severity.Error);

        public MajorRecordAnalyzerResult AnalyzeRecord(IRecordAnalyzerParams<IArmorGetter> param)
        {
            var result = new MajorRecordAnalyzerResult();

            var femaleFile = param.Record.WorldModel?.Female?.Model?.File;
            CheckForMissingAsset(femaleFile, result, () => RecordError.Create(param.Record,
                FormattedErrorDefinition.Create(MissingArmorModel, "female", femaleFile),
                x => x.WorldModel!.Female!.Model!.File));

            var maleFile = param.Record.WorldModel?.Male?.Model?.File;
            CheckForMissingAsset(maleFile, result, () => RecordError.Create(param.Record,
                FormattedErrorDefinition.Create(MissingArmorModel, "male", maleFile),
                x => x.WorldModel!.Male!.Model!.File));

            return result;
        }
    }
}
