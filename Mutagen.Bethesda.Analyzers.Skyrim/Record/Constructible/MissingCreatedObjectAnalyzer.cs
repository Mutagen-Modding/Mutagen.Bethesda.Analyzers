using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Constructible;

public class MissingCreatedObjectAnalyzer : IIsolatedRecordAnalyzer<IConstructibleObjectGetter>
{
    public static readonly TopicDefinition MissingCreatedObject = MutagenTopicBuilder.DevelopmentTopic(
            "Missing Created Object",
            Severity.Warning)
        .WithoutFormatting("Constructible doesn't create any object");

    public IEnumerable<TopicDefinition> Topics { get; } = [MissingCreatedObject];

    public RecordAnalyzerResult? AnalyzeRecord(IsolatedRecordAnalyzerParams<IConstructibleObjectGetter> param)
    {
        if (param.Record.CreatedObject.IsNull)
        {
            return new RecordAnalyzerResult(
                RecordTopic.Create(
                    obj: param.Record,
                    formattedTopicDefinition: MissingCreatedObject.Format(),
                    memberExpression: x => x.CreatedObject
                )
            );
        }

        return null;
    }
}
