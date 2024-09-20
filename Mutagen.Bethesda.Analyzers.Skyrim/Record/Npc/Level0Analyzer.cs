using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Npc;

public class Level0Analyzer : IContextualRecordAnalyzer<INpcGetter>
{
    public static readonly TopicDefinition Level0 = MutagenTopicBuilder.DevelopmentTopic(
            "Level 0",
            Severity.Warning)
        .WithoutFormatting("Npc has level 0");


    public static readonly TopicDefinition MinLevel0 = MutagenTopicBuilder.DevelopmentTopic(
            "Minimum Level 0",
            Severity.Warning)
        .WithoutFormatting("Npc has minimum level 0");

    public static readonly TopicDefinition<float> LevelMultTooSmall = MutagenTopicBuilder.DevelopmentTopic(
            "Level Mult Too Small",
            Severity.Warning)
        .WithFormatting<float>("Npc has too small level mult {0}");


    public IEnumerable<TopicDefinition> Topics { get; } = [Level0];

    public RecordAnalyzerResult? AnalyzeRecord(ContextualRecordAnalyzerParams<INpcGetter> param)
    {
        var npc = param.Record;

        switch (npc.Configuration.Level)
        {
            case INpcLevelGetter npcLevel:
                if (npcLevel.Level == 0)
                {
                    return new RecordAnalyzerResult(
                        RecordTopic.Create(
                            npc,
                            Level0.Format(),
                            x => x.Configuration));
                }
                break;
            case IPcLevelMultGetter pcLevelMult:
                if (Math.Abs(pcLevelMult.LevelMult) < 0.001)
                {
                    return new RecordAnalyzerResult(
                        RecordTopic.Create(
                            npc,
                            LevelMultTooSmall.Format(pcLevelMult.LevelMult),
                            x => x.Configuration));
                }

                if (npc.Configuration.CalcMinLevel == 0)
                {
                    return new RecordAnalyzerResult(
                        RecordTopic.Create(
                            npc,
                            MinLevel0.Format(),
                            x => x.Configuration));
                }
                break;
        }

        return null;
    }
}
