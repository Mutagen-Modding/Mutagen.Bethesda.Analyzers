using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Errors;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.Analyzers.Skyrim
{
    public partial class MissingAssetsAnalyzer : IIsolatedRecordAnalyzer<IStaticGetter>
    {
        public static readonly ErrorDefinition MissingStaticModel = new(
            "SOMEID",
            "Missing Static Model file",
            MissingModelFileMessageFormat,
            Severity.Error);

        public MajorRecordAnalyzerResult AnalyzeRecord(IIsolatedRecordAnalyzerParams<IStaticGetter> param)
        {
            var res = new MajorRecordAnalyzerResult();
            CheckForMissingModelAsset(param.Record, res, MissingStaticModel);
            return res;
        }
    }
}
