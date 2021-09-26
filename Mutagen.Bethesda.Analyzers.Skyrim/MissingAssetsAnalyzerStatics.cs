using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.Analyzers.Skyrim
{
    public partial class MissingAssetsAnalyzer : IRecordAnalyzer<IStaticGetter>
    {
        public static readonly TopicDefinition<string> MissingStaticModel = new(
            "SOMEID",
            "Missing Static Model file",
            MissingModelFileMessageFormat,
            Severity.Error);

        public MajorRecordAnalyzerResult AnalyzeRecord(IRecordAnalyzerParams<IStaticGetter> param)
        {
            var res = new MajorRecordAnalyzerResult();
            CheckForMissingModelAsset(param.Record, res, MissingStaticModel);
            return res;
        }
    }
}
