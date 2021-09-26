using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Errors;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.Analyzers.Skyrim
{
    public partial class MissingAssetsAnalyzer : IRecordAnalyzer<IHeadPartGetter>
    {
        public static readonly ErrorDefinition<string> MissingHeadPartModel = new(
            "SOMEID",
            "Missing Head Part Model file",
            MissingModelFileMessageFormat,
            Severity.Error);

        public static readonly ErrorDefinition<int, string?> MissingHeadPartFile = new(
            "SOMEID",
            "Missing Head Part file",
            "Missing file for Head Part Part {0} at {1}",
            Severity.CTD);

        public MajorRecordAnalyzerResult AnalyzeRecord(IRecordAnalyzerParams<IHeadPartGetter> param)
        {
            var res = new MajorRecordAnalyzerResult();

            CheckForMissingModelAsset(param.Record, res, MissingHeadPartModel);

            var i = 0;
            foreach (var part in param.Record.Parts)
            {
                CheckForMissingAsset(part.FileName, res, () => RecordError.Create(
                    param.Record,
                    MissingHeadPartFile.Format(i, part.FileName),
                    x => x.Parts[0].FileName!));
                i++;
            }

            return res;
        }
    }
}
