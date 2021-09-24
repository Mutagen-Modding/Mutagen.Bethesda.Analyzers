using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Errors;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Skyrim.Internals;

namespace Mutagen.Bethesda.SkyrimAnalyzer
{
    public partial class MissingAssetsAnalyzer : IMajorRecordAnalyzer<IHeadPartGetter>
    {
        public static readonly ErrorDefinition MissingHeadPartModel = new(
            "SOMEID",
            "Missing Head Part Model file",
            "TODO",
            Severity.Error);

        public static readonly ErrorDefinition MissingHeadPartFile = new(
            "SOMEID",
            "Missing Head Part file",
            "TODO",
            Severity.Error);

        public MajorRecordAnalyzerResult AnalyzeRecord(IHeadPartGetter majorRecord)
        {
            var res = new MajorRecordAnalyzerResult();

            CheckForMissingModelAsset(majorRecord, res, MissingHeadPartModel, RecordTypes.HDPT);

            foreach (var part in majorRecord.Parts)
            {
                CheckForMissingAsset(part.FileName, res, () => RecordError.Create(
                    MissingHeadPartFile,
                    majorRecord,
                    RecordTypes.HDPT,
                    x => x.Parts[0].FileName!));
            }

            return res;
        }
    }
}
