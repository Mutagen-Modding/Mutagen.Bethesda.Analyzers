using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Errors;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Skyrim.Internals;

namespace Mutagen.Bethesda.SkyrimAnalyzer
{
    public partial class MissingAssetsAnalyzer : IMajorRecordAnalyzer<IWeaponGetter>
    {
        public static readonly ErrorDefinition MissingWeaponModel = new(
            "SOMEID",
            "Missing Weapon Model file",
            MissingModelFileMessageFormat,
            Severity.Error);

        public MajorRecordAnalyzerResult AnalyzeRecord(IWeaponGetter weapon)
        {
            var res = new MajorRecordAnalyzerResult();
            CheckForMissingModelAsset(weapon, res, MissingWeaponModel);
            return res;
        }
    }
}
