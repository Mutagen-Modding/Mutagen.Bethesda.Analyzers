using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Scroll;

public class MissingFieldsAnalyzer : IIsolatedRecordAnalyzer<IScrollGetter>
{
    public static readonly TopicDefinition MissingVendorKeyword = MutagenTopicBuilder.DevelopmentTopic(
            "Missing Vendor Keyword",
            Severity.Suggestion)
        .WithoutFormatting("Scroll is missing VendorItemScroll keyword");

    public static readonly TopicDefinition EmptyEffectList = MutagenTopicBuilder.DevelopmentTopic(
            "Empty Effect List",
            Severity.Suggestion)
        .WithoutFormatting("Spell has no effect");

    public IEnumerable<TopicDefinition> Topics { get; } = [MissingVendorKeyword, EmptyEffectList];

    public RecordAnalyzerResult AnalyzeRecord(IsolatedRecordAnalyzerParams<IScrollGetter> param)
    {
        var scroll = param.Record;

        var result = new RecordAnalyzerResult();

        if (scroll.Keywords == null || !scroll.Keywords.Contains(FormKeys.SkyrimSE.Skyrim.Keyword.VendorItemScroll))
        {
            result.AddTopic(
                RecordTopic.Create(
                    scroll,
                    MissingVendorKeyword.Format(),
                    x => x.Keywords));
        }

        if (scroll.Effects.Count == 0)
        {
            result.AddTopic(
                RecordTopic.Create(
                    scroll,
                    EmptyEffectList.Format(),
                    x => x.Effects));
        }

        return result;
    }
}
