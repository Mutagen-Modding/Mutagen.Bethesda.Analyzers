using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Errors;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.Analyzers.Skyrim
{
    public partial class MissingAssetsAnalyzer : IRecordAnalyzer<IWeaponGetter>
    {
        public static readonly ErrorDefinition MissingWeaponModel = new(
            "SOMEID",
            "Missing Weapon Model file",
            MissingModelFileMessageFormat,
            Severity.Error);

        public MajorRecordAnalyzerResult AnalyzeRecord(IRecordAnalyzerParams<IWeaponGetter> param)
        {
            var res = new MajorRecordAnalyzerResult();
            CheckForMissingModelAsset(param.Record, res, MissingWeaponModel);
            return res;
        }
    }
}
