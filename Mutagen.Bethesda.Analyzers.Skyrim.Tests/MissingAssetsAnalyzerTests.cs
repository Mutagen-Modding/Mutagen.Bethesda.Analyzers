using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using Microsoft.Extensions.Logging.Abstractions;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Analyzers.TestingUtils;
using Mutagen.Bethesda.Plugins.Assets;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Skyrim.Assets;
using Mutagen.Bethesda.Testing.AutoData;
using Noggog;
using NSubstitute;
using Xunit;

namespace Mutagen.Bethesda.Analyzers.Skyrim.Tests;

public class MissingAssetsAnalyzerTests
{
    [Theory, MutagenModAutoData]
    public void TestMissingArmorModel(
        IFileSystem fileSystem,
        Armor armorRecord,
        MissingAssetsAnalyzer sut)
    {
        armorRecord.WorldModel = new GenderedItem<ArmorModel?>(CreateArmorModel(), CreateArmorModel());

        var result = sut.AnalyzeRecord(armorRecord.AsIsolatedParams<IArmorGetter>());
        AnalyzerTestUtils.HasTopic(result, MissingAssetsAnalyzer.MissingArmorModel, 2);
    }

    [Theory, MutagenModAutoData]
    public void TestMissingTextureSetTextures(
        IFileSystem fileSystem,
        TextureSet textureSetRecord,
        MissingAssetsAnalyzer sut)
    {
        textureSetRecord.Diffuse = new AssetLink<SkyrimTextureAssetType>(Path.GetRandomFileName());
        textureSetRecord.NormalOrGloss = new AssetLink<SkyrimTextureAssetType>(Path.GetRandomFileName());
        textureSetRecord.EnvironmentMaskOrSubsurfaceTint = new AssetLink<SkyrimTextureAssetType>(Path.GetRandomFileName());
        textureSetRecord.GlowOrDetailMap = new AssetLink<SkyrimTextureAssetType>(Path.GetRandomFileName());
        textureSetRecord.Height = new AssetLink<SkyrimTextureAssetType>(Path.GetRandomFileName());
        textureSetRecord.Environment = new AssetLink<SkyrimTextureAssetType>(Path.GetRandomFileName());
        textureSetRecord.Multilayer = new AssetLink<SkyrimTextureAssetType>(Path.GetRandomFileName());
        textureSetRecord.BacklightMaskOrSpecular = new AssetLink<SkyrimTextureAssetType>(Path.GetRandomFileName());

        var result = sut.AnalyzeRecord(textureSetRecord.AsIsolatedParams<ITextureSetGetter>());
        AnalyzerTestUtils.HasTopic(result, MissingAssetsAnalyzer.MissingTextureInTextureSet, 8);
    }

    [Theory, MutagenAutoData]
    public void TestExistingTextureSetTextures(
        ITextureSetGetter textureSet,
        AssetLink<SkyrimTextureAssetType> existingDiffuse,
        AssetLink<SkyrimTextureAssetType> existingNormal,
        AssetLink<SkyrimTextureAssetType> existingSubsurfaceTint,
        AssetLink<SkyrimTextureAssetType> existingGlow,
        AssetLink<SkyrimTextureAssetType> existingHeight,
        AssetLink<SkyrimTextureAssetType> existingEnvironment,
        AssetLink<SkyrimTextureAssetType> existingMultilayer,
        AssetLink<SkyrimTextureAssetType> existingSpecular,
        MissingAssetsAnalyzer analyzer)
    {
        textureSet.Diffuse.Returns(existingDiffuse);
        textureSet.NormalOrGloss.Returns(existingNormal);
        textureSet.EnvironmentMaskOrSubsurfaceTint.Returns(existingSubsurfaceTint);
        textureSet.GlowOrDetailMap.Returns(existingGlow);
        textureSet.Height.Returns(existingHeight);
        textureSet.Environment.Returns(existingEnvironment);
        textureSet.Multilayer.Returns(existingMultilayer);
        textureSet.BacklightMaskOrSpecular.Returns(existingSpecular);

        var result = analyzer.AnalyzeRecord(textureSet.AsIsolatedParams());
        Assert.Empty(result.Topics);
    }

    [Theory, MutagenModAutoData]
    public void TestMissingWeaponModel(Weapon weapon)
    {
        TestMissingModelFile(weapon, x => x.AnalyzeRecord(weapon.AsIsolatedParams<IWeaponGetter>()), MissingAssetsAnalyzer.MissingWeaponModel);
    }

    [Theory, MutagenAutoData]
    public void TestExistingWeaponModel(
        IWeaponGetter weapon,
        IFileSystem fs,
        AssetLink<SkyrimModelAssetType> existingModelFile,
        MissingAssetsAnalyzer analyzer)
    {
        weapon.Model.Returns(new Model
        {
            File = existingModelFile
        });

        var result = analyzer.AnalyzeRecord(weapon.AsIsolatedParams());
        Assert.Empty(result.Topics);
    }

    [Theory, MutagenModAutoData]
    public void TestMissingStaticsModel(Static stat)
    {
        TestMissingModelFile(stat, x => x.AnalyzeRecord(stat.AsIsolatedParams<IStaticGetter>()), MissingAssetsAnalyzer.MissingStaticModel);
    }

    [Theory, MutagenAutoData]
    public void TestExistingStaticsModel(
        IStaticGetter staticGetter,
        AssetLink<SkyrimModelAssetType> existingModelFile,
        MissingAssetsAnalyzer analyzer)
    {
        staticGetter.Model.Returns(new Model
        {
            File = existingModelFile
        });

        var result = analyzer.AnalyzeRecord(staticGetter.AsIsolatedParams());
        Assert.Empty(result.Topics);
    }

    [Theory, MutagenModAutoData]
    public void TestMissingHeadPartModel(HeadPart headPart)
    {
        TestMissingModelFile(headPart, x => x.AnalyzeRecord(headPart.AsIsolatedParams<IHeadPartGetter>()), MissingAssetsAnalyzer.MissingHeadPartModel);
    }

    [Theory, MutagenAutoData]
    public void TestExistingHeadPartModel(
        IHeadPartGetter headPart,
        AssetLink<SkyrimModelAssetType> existingModelFile,
        MissingAssetsAnalyzer analyzer)
    {
        headPart.Model.Returns(new Model
        {
            File = existingModelFile
        });

        var result = analyzer.AnalyzeRecord(headPart.AsIsolatedParams());
        Assert.Empty(result.Topics);
    }

    [Theory, MutagenModAutoData]
    public void TestMissingHeadPartFile(
        IFileSystem fileSystem,
        HeadPart headPart,
        MissingAssetsAnalyzer sut)
    {
        headPart.Parts.Add(
            new Part
            {
                FileName = Path.GetRandomFileName()
            });

        var result = sut.AnalyzeRecord(headPart.AsIsolatedParams<IHeadPartGetter>());
        AnalyzerTestUtils.HasTopic(result, MissingAssetsAnalyzer.MissingHeadPartFile);
    }

    private static void TestMissingModelFile<TMajorRecordGetter>(
        TMajorRecordGetter mock,
        Func<MissingAssetsAnalyzer, RecordAnalyzerResult> func,
        TopicDefinition<string> topicDefinition)
        where TMajorRecordGetter : class, IMajorRecordGetter, IModeled
    {
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
        var analyzer = new MissingAssetsAnalyzer(NullLogger<MissingAssetsAnalyzer>.Instance, fileSystem);

        mock.Model = new Model
        {
            File = Path.GetRandomFileName()
        };

        var res = func(analyzer);
        AnalyzerTestUtils.HasTopic(res, topicDefinition);
    }

    private static ArmorModel CreateArmorModel()
    {
        return new ArmorModel
        {
            Model = new Model
            {
                File = Path.GetRandomFileName()
            }
        };
    }
}
