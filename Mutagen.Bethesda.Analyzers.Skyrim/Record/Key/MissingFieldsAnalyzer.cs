using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Key;

public class MissingFieldsAnalyzer : IIsolatedRecordAnalyzer<IKeyGetter>
{
    public static readonly TopicDefinition NoPickupSound = MutagenTopicBuilder.FromDiscussion(
            0,
            "No Pickup Sound",
            Severity.Suggestion)
        .WithoutFormatting("Key has no pickup sound");

    public static readonly TopicDefinition NoPutDownSound = MutagenTopicBuilder.FromDiscussion(
            0,
            "No Put Down Sound",
            Severity.Suggestion)
        .WithoutFormatting("Key has no put down sound");

    public static readonly TopicDefinition MissingVendorItemKeyword = MutagenTopicBuilder.FromDiscussion(
            0,
            "Missing VendorItemKey Keyword",
            Severity.Suggestion)
        .WithoutFormatting("Key is missing the VendorItemKey keyword");

    public IEnumerable<TopicDefinition> Topics { get; } = [NoPickupSound, NoPutDownSound];

    public RecordAnalyzerResult AnalyzeRecord(IsolatedRecordAnalyzerParams<IKeyGetter> param)
    {
        var key = param.Record;

        var result = new RecordAnalyzerResult();

        if (key.PickUpSound.IsNull)
        {
            result.AddTopic(
                RecordTopic.Create(
                    key,
                    NoPickupSound.Format(),
                    x => x.PickUpSound
                )
            );
        }

        if (key.PutDownSound.IsNull)
        {
            result.AddTopic(
                RecordTopic.Create(
                    key,
                    NoPutDownSound.Format(),
                    x => x.PutDownSound
                )
            );
        }

        return result;
    }
}
