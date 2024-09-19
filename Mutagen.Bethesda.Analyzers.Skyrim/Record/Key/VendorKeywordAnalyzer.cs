using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Key;

public class VendorKeywordAnalyzer : IIsolatedRecordAnalyzer<IKeyGetter>
{
    public static readonly TopicDefinition MissingVendorItemKeyword = MutagenTopicBuilder.FromDiscussion(
            0,
            "Missing VendorItemKey Keyword",
            Severity.Suggestion)
        .WithoutFormatting("Key is missing the VendorItemKey keyword");

    public IEnumerable<TopicDefinition> Topics { get; } = [MissingVendorItemKeyword];

    public RecordAnalyzerResult AnalyzeRecord(IsolatedRecordAnalyzerParams<IKeyGetter> param)
    {
        var key = param.Record;

        var result = new RecordAnalyzerResult();

        if (key.Keywords is null || !key.Keywords.Contains(FormKeys.SkyrimSE.Skyrim.Keyword.VendorItemKey))
        {
            result.AddTopic(
                RecordTopic.Create(
                    key,
                    MissingVendorItemKeyword.Format(),
                    x => x.Keywords
                )
            );
        }

        return result;
    }
}
