using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Npc.Unique;

public class NoVoiceTypeAnalyzer : IContextualRecordAnalyzer<INpcGetter>
{
    public static readonly TopicDefinition NoVoiceType = MutagenTopicBuilder.DevelopmentTopic(
            "No Voice Type",
            Severity.Warning)
        .WithoutFormatting("Npc has no voice type");

    public IEnumerable<TopicDefinition> Topics { get; } = [NoVoiceType];

    public RecordAnalyzerResult? AnalyzeRecord(ContextualRecordAnalyzerParams<INpcGetter> param)
    {
        var npc = param.Record;
        if (!npc.IsEligibleForTest()) return null;

        // Skip NPCs using templates for voice types
        if (npc.Configuration.TemplateFlags.HasFlag(NpcConfiguration.TemplateFlag.Traits)) return null;

        if (npc.Voice.IsNull)
        {
            return new RecordAnalyzerResult(
                RecordTopic.Create(
                    npc,
                    NoVoiceType.Format(),
                    x => x.Configuration));
        }

        return null;
    }
}
