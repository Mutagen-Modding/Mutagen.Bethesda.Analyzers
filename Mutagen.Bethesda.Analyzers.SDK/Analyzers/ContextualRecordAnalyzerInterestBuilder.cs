using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.SDK.Analyzers;

public class ContextualRecordAnalyzerInterestBuilder<TObj>
{
    public ContextualRecordAnalyzerInterestBuilder<TRet> Add<TRet>(Func<TObj, TRet> selector)
    {
        return null!;
    }

    public ContextualRecordAnalyzerInterestBuilder<TRet> AddList<TRet>(Func<TObj, IEnumerable<TRet>> selector)
    {
        return null!;
    }

    public ContextualRecordAnalyzerInterestBuilder<TMajor> AllFormLinksOfType<TMajor>()
        where TMajor : IMajorRecordQueryableGetter
    {
        return null!;
    }
}

