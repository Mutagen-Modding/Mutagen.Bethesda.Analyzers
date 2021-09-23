using Mutagen.Bethesda.Analyzers.SDK.Errors;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Skyrim.Internals;

namespace Mutagen.Bethesda.SkyrimAnalyzer
{
    public partial class MissingAssetsAnalyzer
    {
        public static readonly ErrorDefinition MissingStaticModel = new(
            "SOMEID",
            "Missing Static Model file",
            "TODO",
            Severity.Error);

        public MajorRecordAnalyzerResult AnalyzeRecord(IStaticGetter staticGetter)
        {
            var res = new MajorRecordAnalyzerResult();
            CheckForMissingModelAsset(staticGetter, res, MissingStaticModel, RecordTypes.STAT);
            return res;
        }
    }
}
