using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.Analyzers.Skyrim.Record.Faction;

public class VendorHourAnalyzer : IIsolatedRecordAnalyzer<IFactionGetter>
{
    public static readonly TopicDefinition<ushort, ushort> WrongVendorHourOrder = MutagenTopicBuilder.DevelopmentTopic(
            "Wrong Vendor Hour Order",
            Severity.Error)
        .WithFormatting<ushort, ushort>("Vendor hours are set to start at {0} and end at {1}, but the start hour is after the end hour, this vendor will never be available");

    public IEnumerable<TopicDefinition> Topics { get; } = [WrongVendorHourOrder];

    public void AnalyzeRecord(IsolatedRecordAnalyzerParams<IFactionGetter> param)
    {
        var faction = param.Record;

        if (!faction.IsVendor()) return;
        if (faction.VendorValues is null) return;


        if (faction.VendorValues.StartHour < faction.VendorValues.EndHour)
        {
            return;
        }

        param.AddTopic(
            WrongVendorHourOrder.Format(faction.VendorValues.StartHour, faction.VendorValues.EndHour),
            x => x.VendorValues);
    }
}
