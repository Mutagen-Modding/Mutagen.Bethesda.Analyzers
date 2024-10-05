using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
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

    public void AnalyzeRecord(IsolatedRecordAnalyzerParams<IScrollGetter> param)
    {
        var scroll = param.Record;

        if (scroll.Keywords == null || !scroll.Keywords.Contains(FormKeys.SkyrimSE.Skyrim.Keyword.VendorItemScroll))
        {
            param.AddTopic(
                MissingVendorKeyword.Format(),
                x => x.Keywords);
        }

        if (scroll.Effects.Count == 0)
        {
            param.AddTopic(
                EmptyEffectList.Format(),
                x => x.Effects);
        }
    }
}
