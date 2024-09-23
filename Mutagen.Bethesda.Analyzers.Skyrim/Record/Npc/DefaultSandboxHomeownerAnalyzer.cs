using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Npc;

public class DefaultSandboxHomeownerAnalyzer : IContextualRecordAnalyzer<INpcGetter>
{
    public static readonly TopicDefinition DefaultSandboxHomeownerListLast = MutagenTopicBuilder.DevelopmentTopic(
            "Last package is DefaultSandboxHomeowner",
            Severity.Suggestion)
        .WithoutFormatting("Npc uses DefaultSandboxHomeowner as last package, consider using DefaultHomeOwnerPackageList in Default Package List instead");

    public IEnumerable<TopicDefinition> Topics { get; } = [];

    public RecordAnalyzerResult? AnalyzeRecord(ContextualRecordAnalyzerParams<INpcGetter> param)
    {
        var npc = param.Record;
        if (npc.Packages.Count == 0) return null;
        if (!npc.DefaultPackageList.IsNull && npc.DefaultPackageList.FormKey == FormKeys.SkyrimSE.Skyrim.FormList.DefaultHomeOwnerPackageList.FormKey) return null;

        if (npc.Packages[^1].FormKey == FormKeys.SkyrimSE.Skyrim.Package.DefaultSandboxHomeowner.FormKey)
        {
            return new RecordAnalyzerResult(
                RecordTopic.Create(
                    npc,
                    DefaultSandboxHomeownerListLast.Format(),
                    x => x.Packages));
        }

        return null;
    }
}
