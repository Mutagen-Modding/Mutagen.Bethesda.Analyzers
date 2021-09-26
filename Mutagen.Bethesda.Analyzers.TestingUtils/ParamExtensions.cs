using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.TestingUtils
{
    public static class ParamExtensions
    {
        public static IRecordAnalyzerParams<TMajor> AsBasicParams<TMajor>(this TMajor maj)
            where TMajor : IMajorRecordGetter
        {
            return new RecordAnalyzerParams<TMajor>(null!, null!)
            {
                Record = maj
            };
        }
    }
}
