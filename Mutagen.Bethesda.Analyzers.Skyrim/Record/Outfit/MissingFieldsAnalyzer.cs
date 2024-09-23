using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Outfit;

public class MissingFieldsAnalyzer : IIsolatedRecordAnalyzer<IOutfitGetter>
{
    public static readonly TopicDefinition NoItems = MutagenTopicBuilder.DevelopmentTopic(
            "No Items",
            Severity.Warning)
        .WithoutFormatting("Outfit has no items");

    public IEnumerable<TopicDefinition> Topics { get; } = [NoItems];

    public RecordAnalyzerResult AnalyzeRecord(IsolatedRecordAnalyzerParams<IOutfitGetter> param)
    {
        var outfit = param.Record;

        var result = new RecordAnalyzerResult();

        if (outfit.Items is not null && outfit.Items.Count == 0)
        {
            result.AddTopic(
                RecordTopic.Create(
                    outfit,
                    NoItems.Format(),
                    x => x.Items
                )
            );
        }

        return result;
    }
}
