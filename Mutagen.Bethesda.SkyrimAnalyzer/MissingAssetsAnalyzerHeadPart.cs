using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Errors;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.SkyrimAnalyzer
{
    public partial class MissingAssetsAnalyzer : IMajorRecordAnalyzer<IHeadPartGetter>
    {
        public static readonly ErrorDefinition MissingHeadPartModel = new(
            "SOMEID",
            "Missing Head Part Model file",
            MissingModelFileMessageFormat,
            Severity.Error);

        public static readonly ErrorDefinition MissingHeadPartFile = new(
            "SOMEID",
            "Missing Head Part file",
            "Missing file for Head Part Part {0} at {1}",
            Severity.Error);

        public MajorRecordAnalyzerResult AnalyzeRecord(IHeadPartGetter headPart)
        {
            var res = new MajorRecordAnalyzerResult();

            CheckForMissingModelAsset(headPart, res, MissingHeadPartModel);

            var i = 0;
            foreach (var part in headPart.Parts)
            {
                CheckForMissingAsset(part.FileName, res, () => RecordError.Create(
                    headPart,
                    FormattedErrorDefinition.Create(
                        MissingHeadPartFile,
                        i, part.FileName),
                    x => x.Parts[0].FileName!));
                i++;
            }

            return res;
        }
    }
}
