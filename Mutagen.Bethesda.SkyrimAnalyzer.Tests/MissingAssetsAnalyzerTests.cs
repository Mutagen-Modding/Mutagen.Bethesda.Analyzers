using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Mutagen.Bethesda.Analyzers.Testing.AutoFixture;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.Skyrim;
using Noggog;
using Xunit;

namespace Mutagen.Bethesda.SkyrimAnalyzer.Tests
{
    public class MissingAssetsAnalyzerTests
    {
        [Fact]
        public void TestMissingArmorModel()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
            var analyzer = new MissingAssetsAnalyzer(NullLogger<MissingAssetsAnalyzer>.Instance, fileSystem);

            var armorRecord = Mock.Of<IArmorGetter>();
            Mock.Get(armorRecord)
                .Setup(x => x.WorldModel)
                .Returns(() => new GenderedItem<IArmorModelGetter?>(CreateArmorModel(), CreateArmorModel()));

            var result = analyzer.AnalyzeRecord(armorRecord);
            Assert.Collection(result.Errors,
                x => Assert.Equal(MissingAssetsAnalyzer.MissingArmorModel, x.ErrorDefinition),
                x => Assert.Equal(MissingAssetsAnalyzer.MissingArmorModel, x.ErrorDefinition));
        }

        [Fact]
        public void TestMissingTextureSetTextures()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
            var analyzer = new MissingAssetsAnalyzer(NullLogger<MissingAssetsAnalyzer>.Instance, fileSystem);

            var textureSetRecord = Mock.Of<ITextureSetGetter>();
            var mock = Mock.Get(textureSetRecord);

            mock.Setup(x => x.Diffuse)
                .Returns(Path.GetRandomFileName());
            mock.Setup(x => x.NormalOrGloss)
                .Returns(Path.GetRandomFileName());
            mock.Setup(x => x.EnvironmentMaskOrSubsurfaceTint)
                .Returns(Path.GetRandomFileName());
            mock.Setup(x => x.GlowOrDetailMap)
                .Returns(Path.GetRandomFileName());
            mock.Setup(x => x.Height)
                .Returns(Path.GetRandomFileName());
            mock.Setup(x => x.Environment)
                .Returns(Path.GetRandomFileName());
            mock.Setup(x => x.Multilayer)
                .Returns(Path.GetRandomFileName());
            mock.Setup(x => x.BacklightMaskOrSpecular)
                .Returns(Path.GetRandomFileName());

            var result = analyzer.AnalyzeRecord(textureSetRecord);
            Assert.Collection(
                result.Errors,
                x => Assert.Equal(MissingAssetsAnalyzer.MissingTextureInTextureSet, x.ErrorDefinition),
                x => Assert.Equal(MissingAssetsAnalyzer.MissingTextureInTextureSet, x.ErrorDefinition),
                x => Assert.Equal(MissingAssetsAnalyzer.MissingTextureInTextureSet, x.ErrorDefinition),
                x => Assert.Equal(MissingAssetsAnalyzer.MissingTextureInTextureSet, x.ErrorDefinition),
                x => Assert.Equal(MissingAssetsAnalyzer.MissingTextureInTextureSet, x.ErrorDefinition),
                x => Assert.Equal(MissingAssetsAnalyzer.MissingTextureInTextureSet, x.ErrorDefinition),
                x => Assert.Equal(MissingAssetsAnalyzer.MissingTextureInTextureSet, x.ErrorDefinition),
                x => Assert.Equal(MissingAssetsAnalyzer.MissingTextureInTextureSet, x.ErrorDefinition));
        }

        [Theory, MoqData]
        public void TestExistingTextureSetTextures(
            Mock<ITextureSetGetter> textureSet,
            FilePath existingDiffuse,
            FilePath existingNormal,
            FilePath existingSubsurfaceTint,
            FilePath existingGlow,
            FilePath existingHeight,
            FilePath existingEnvironment,
            FilePath existingMultilayer,
            FilePath existingSpecular,
            MissingAssetsAnalyzer analyzer)
        {
            textureSet.Setup(x => x.Diffuse)
                .Returns(existingDiffuse);
            textureSet.Setup(x => x.NormalOrGloss)
                .Returns(existingNormal);
            textureSet.Setup(x => x.EnvironmentMaskOrSubsurfaceTint)
                .Returns(existingSubsurfaceTint);
            textureSet.Setup(x => x.GlowOrDetailMap)
                .Returns(existingGlow);
            textureSet.Setup(x => x.Height)
                .Returns(existingHeight);
            textureSet.Setup(x => x.Environment)
                .Returns(existingEnvironment);
            textureSet.Setup(x => x.Multilayer)
                .Returns(existingMultilayer);
            textureSet.Setup(x => x.BacklightMaskOrSpecular)
                .Returns(existingSpecular);

            var result = analyzer.AnalyzeRecord(textureSet.Object);

            result.Errors.Should().BeEmpty();
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
}
