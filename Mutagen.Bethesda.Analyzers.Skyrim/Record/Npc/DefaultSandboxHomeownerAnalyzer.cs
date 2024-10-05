using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
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

    public void AnalyzeRecord(ContextualRecordAnalyzerParams<INpcGetter> param)
    {
        var npc = param.Record;
        if (npc.Packages.Count == 0) return;
        if (!npc.DefaultPackageList.IsNull && npc.DefaultPackageList.FormKey == FormKeys.SkyrimSE.Skyrim.FormList.DefaultHomeOwnerPackageList.FormKey) return;

        if (npc.Packages[^1].FormKey == FormKeys.SkyrimSE.Skyrim.Package.DefaultSandboxHomeowner.FormKey)
        {
            param.AddTopic(
                DefaultSandboxHomeownerListLast.Format(),
                x => x.Packages);
        }
    }
}
