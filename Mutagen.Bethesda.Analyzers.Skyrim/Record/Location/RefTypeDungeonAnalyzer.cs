using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Location;

public class RefTypeDungeonAnalyzer : IContextualRecordAnalyzer<ILocationGetter>
{
    public static readonly TopicDefinition NoBossRefType = MutagenTopicBuilder.DevelopmentTopic(
            "No Boss",
            Severity.Suggestion)
        .WithoutFormatting("Dungeon location has no Boss Ref Type - not set up for radiant quests");

    public static readonly TopicDefinition NoBossContainerRefType = MutagenTopicBuilder.DevelopmentTopic(
            "No Boss Container",
            Severity.Suggestion)
        .WithoutFormatting("Dungeon location has no Boss Container Ref Type - not set up for radiant quests");

    public IEnumerable<TopicDefinition> Topics { get; } = [NoBossRefType, NoBossContainerRefType];

    public RecordAnalyzerResult AnalyzeRecord(ContextualRecordAnalyzerParams<ILocationGetter> param)
    {
        var location = param.Record;
        var result = new RecordAnalyzerResult();

        if (location.Keywords is null || location.Keywords.All(k => k.FormKey != FormKeys.SkyrimSE.Skyrim.Keyword.LocTypeDungeon.FormKey)) return result;

        var referenceTypes = location.GetReferenceTypes().ToList();

        if (!referenceTypes.Exists(staticRef => staticRef.LocationRefType.FormKey == FormKeys.SkyrimSE.Skyrim.LocationReferenceType.Boss.FormKey))
        {
            result.AddTopic(
                RecordTopic.Create(
                    location,
                    NoBossRefType.Format(),
                    x => x.LocationCellStaticReferences));
        }

        if (!referenceTypes.Exists(staticRef => staticRef.LocationRefType.FormKey == FormKeys.SkyrimSE.Skyrim.LocationReferenceType.BossContainer.FormKey))
        {
            result.AddTopic(
                RecordTopic.Create(
                    location,
                    NoBossContainerRefType.Format(),
                    x => x.LocationCellStaticReferences));
        }

        return result;
    }
}
