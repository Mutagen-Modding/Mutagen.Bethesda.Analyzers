using System.Text.RegularExpressions;
using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Package;

public partial class InconsistentTimeframeAnalyzer : IContextualRecordAnalyzer<IPackageGetter>
{
    public static readonly TopicDefinition<string, string> InconsistentTimeframeTopic = MutagenTopicBuilder.DevelopmentTopic(
            "Inconsistent Timeframe",
            Severity.Suggestion)
        .WithFormatting<string, string>("The timeframe in the package data and the editor id should be the same");

    public IEnumerable<TopicDefinition> Topics { get; } = [InconsistentTimeframeTopic];


    [GeneratedRegex(@"[^\d\s]*(\d+)x(\d+)")]
    private static partial Regex TimeframeRegex();

    public RecordAnalyzerResult? AnalyzeRecord(ContextualRecordAnalyzerParams<IPackageGetter> param)
    {
        var package = param.Record;

        if (!TryGetEditorIDTimeframe(package, out var hour, out var duration)) return null;
        if (package is { ScheduleHour: -1, ScheduleDurationInMinutes: 0 } && hour == 0 && duration % 24 == 0) return null;

        if (package.ScheduleHour != hour || package.ScheduleDurationInMinutes / 60 != duration)
        {
            return new RecordAnalyzerResult(
                RecordTopic.Create(
                    package,
                    InconsistentTimeframeTopic.Format(),
                    x => x));
        }

        return null;
    }

    private static bool TryGetEditorIDTimeframe(IPackageGetter package, out int hour, out int duration)
    {
        if (package.EditorID is null)
        {
            hour = 0;
            duration = 0;
            return false;
        }

        var match = TimeframeRegex().Match(package.EditorID);
        if (!match.Success || match.Groups.Count < 3)
        {
            hour = 0;
            duration = 0;
            return false;
        }

        hour = Convert.ToInt32(match.Groups[1].Value);
        duration = Convert.ToInt32(match.Groups[2].Value);
        return true;
    }
}
