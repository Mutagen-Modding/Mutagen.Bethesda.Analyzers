using System.Text.RegularExpressions;
using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Package;

public partial class InconsistentTimeframeAnalyzer : IContextualRecordAnalyzer<IPackageGetter>
{
    public static readonly TopicDefinition<int, int> InconsistentHourTopic = MutagenTopicBuilder.DevelopmentTopic(
            "Inconsistent Timeframe",
            Severity.Suggestion)
        .WithFormatting<int, int>("Starting hour {0} doesn't match starting hour in the editor id {1}");

    public static readonly TopicDefinition<int, int> InconsistentDurationTopic = MutagenTopicBuilder.DevelopmentTopic(
            "Inconsistent Timeframe",
            Severity.Suggestion)
        .WithFormatting<int, int>("Duration {0} doesn't match duration in the editor id {1}");

    public IEnumerable<TopicDefinition> Topics { get; } = [InconsistentHourTopic, InconsistentDurationTopic];


    [GeneratedRegex(@"[^\d\s]*(\d+)x(\d+)")]
    private static partial Regex TimeframeRegex();

    public void AnalyzeRecord(ContextualRecordAnalyzerParams<IPackageGetter> param)
    {
        var package = param.Record;

        if (!TryGetEditorIDTimeframe(package, out var hour, out var duration)) return;
        if (package is { ScheduleHour: -1, ScheduleDurationInMinutes: 0 } && hour == 0 && duration % 24 == 0) return;

        if (package.ScheduleHour != hour)
        {
            param.AddTopic(
                InconsistentHourTopic.Format(package.ScheduleHour, hour),
                x => x.ScheduleHour);
        }
        if (package.ScheduleDurationInMinutes / 60 != duration)
        {
            param.AddTopic(
                InconsistentDurationTopic.Format(package.ScheduleDurationInMinutes / 60, duration),
                x => x.ScheduleDurationInMinutes);
        }
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
