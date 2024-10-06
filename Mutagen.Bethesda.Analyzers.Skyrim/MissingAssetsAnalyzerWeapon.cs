using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.Analyzers.Skyrim;

public partial class MissingAssetsAnalyzer : IIsolatedRecordAnalyzer<IWeaponGetter>
{
    public static readonly TopicDefinition<string> MissingWeaponModel = MutagenTopicBuilder.FromDiscussion(
            92,
            "Missing Weapon Model file",
            Severity.Error)
        .WithFormatting<string>(MissingModelFileMessageFormat);

    public void AnalyzeRecord(IsolatedRecordAnalyzerParams<IWeaponGetter> param)
    {
        CheckForMissingModelAsset(param, MissingWeaponModel);
    }

    public IEnumerable<Func<IWeaponGetter, object?>> FieldsOfInterest()
    {
        yield return x => x.Model!.File;
    }
}
