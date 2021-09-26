﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.Testing;
using Mutagen.Bethesda.Analyzers.Testing.AutoFixture;
using Mutagen.Bethesda.Analyzers.TestingUtils;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.Skyrim;
using Noggog;
using Xunit;

namespace Mutagen.Bethesda.Analyzers.Skyrim.Tests
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

            var result = analyzer.AnalyzeRecord(armorRecord.AsBasicParams());
            AnalyzerTestUtils.HasTopic(result, MissingAssetsAnalyzer.MissingArmorModel, 2);
        }

        [Fact]
        public void TestMissingTextureSetTextures()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
            var analyzer = new MissingAssetsAnalyzer(NullLogger<MissingAssetsAnalyzer>.Instance, fileSystem);

            var textureSetRecord = Mock.Of<ITextureSetGetter>();
            var mock = Mock.Get(textureSetRecord);

            mock.Setup(x => x.Diffuse).Returns(Path.GetRandomFileName());
            mock.Setup(x => x.NormalOrGloss).Returns(Path.GetRandomFileName());
            mock.Setup(x => x.EnvironmentMaskOrSubsurfaceTint).Returns(Path.GetRandomFileName());
            mock.Setup(x => x.GlowOrDetailMap).Returns(Path.GetRandomFileName());
            mock.Setup(x => x.Height).Returns(Path.GetRandomFileName());
            mock.Setup(x => x.Environment).Returns(Path.GetRandomFileName());
            mock.Setup(x => x.Multilayer).Returns(Path.GetRandomFileName());
            mock.Setup(x => x.BacklightMaskOrSpecular).Returns(Path.GetRandomFileName());

            var result = analyzer.AnalyzeRecord(textureSetRecord.AsBasicParams());
            AnalyzerTestUtils.HasTopic(result, MissingAssetsAnalyzer.MissingTextureInTextureSet, 8);
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
            textureSet.Setup(x => x.Diffuse).Returns(existingDiffuse);
            textureSet.Setup(x => x.NormalOrGloss).Returns(existingNormal);
            textureSet.Setup(x => x.EnvironmentMaskOrSubsurfaceTint).Returns(existingSubsurfaceTint);
            textureSet.Setup(x => x.GlowOrDetailMap).Returns(existingGlow);
            textureSet.Setup(x => x.Height).Returns(existingHeight);
            textureSet.Setup(x => x.Environment).Returns(existingEnvironment);
            textureSet.Setup(x => x.Multilayer).Returns(existingMultilayer);
            textureSet.Setup(x => x.BacklightMaskOrSpecular).Returns(existingSpecular);

            var result = analyzer.AnalyzeRecord(textureSet.Object.AsBasicParams());
            Assert.Empty(result.Topics);
        }

        [Fact]
        public void TestMissingWeaponModel()
        {
            var weapon = Mock.Of<IWeaponGetter>();
            TestMissingModelFile(weapon, x => x.AnalyzeRecord(weapon.AsBasicParams()), MissingAssetsAnalyzer.MissingWeaponModel);
        }

        [Theory, MoqData]
        public void TestExistingWeaponModel(
            Mock<IWeaponGetter> weapon,
            FilePath existingModelFile,
            MissingAssetsAnalyzer analyzer)
        {
            weapon.Setup(x => x.Model).Returns(() => new Model
            {
                File = existingModelFile
            });

            var result = analyzer.AnalyzeRecord(weapon.Object.AsBasicParams());
            Assert.Empty(result.Topics);
        }

        [Fact]
        public void TestMissingStaticsModel()
        {
            var staticGetter = Mock.Of<IStaticGetter>();
            TestMissingModelFile(staticGetter, x => x.AnalyzeRecord(staticGetter.AsBasicParams()), MissingAssetsAnalyzer.MissingStaticModel);
        }

        [Theory, MoqData]
        public void TestExistingStaticsModel(
            Mock<IStaticGetter> staticGetter,
            FilePath existingModelFile,
            MissingAssetsAnalyzer analyzer)
        {
            staticGetter.Setup(x => x.Model).Returns(() => new Model
            {
                File = existingModelFile
            });

            var result = analyzer.AnalyzeRecord(staticGetter.Object.AsBasicParams());
            Assert.Empty(result.Topics);
        }

        [Fact]
        public void TestMissingHeadPartModel()
        {
            var headPart = Mock.Of<IHeadPartGetter>();
            Mock.Get(headPart)
                .Setup(x => x.Parts)
                .Returns(() => new List<IPartGetter>());
            TestMissingModelFile(headPart, x => x.AnalyzeRecord(headPart.AsBasicParams()), MissingAssetsAnalyzer.MissingHeadPartModel);
        }

        [Theory, MoqData]
        public void TestExistingHeadPartModel(
            Mock<IHeadPartGetter> headPart,
            FilePath existingModelFile,
            MissingAssetsAnalyzer analyzer)
        {
            headPart.Setup(x => x.Model).Returns(() => new Model
            {
                File = existingModelFile
            });

            var result = analyzer.AnalyzeRecord(headPart.Object.AsBasicParams());
            Assert.Empty(result.Topics);
        }

        [Fact]
        public void TestMissingHeadPartFile()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
            var analyzer = new MissingAssetsAnalyzer(NullLogger<MissingAssetsAnalyzer>.Instance, fileSystem);

            var headPart = Mock.Of<IHeadPartGetter>();
            Mock.Get(headPart)
                .Setup(x => x.Parts)
                .Returns(() => new List<IPartGetter>
                {
                    new Part
                    {
                        FileName = Path.GetRandomFileName()
                    }
                });

            var result = analyzer.AnalyzeRecord(headPart.AsBasicParams());
            AnalyzerTestUtils.HasTopic(result, MissingAssetsAnalyzer.MissingHeadPartFile);
        }

        private static void TestMissingModelFile<TMajorRecordGetter>(
            TMajorRecordGetter mock,
            Func<MissingAssetsAnalyzer, MajorRecordAnalyzerResult> func,
            TopicDefinition<string> topicDefinition)
            where TMajorRecordGetter : class, IMajorRecordGetter, IModeledGetter
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>());
            var analyzer = new MissingAssetsAnalyzer(NullLogger<MissingAssetsAnalyzer>.Instance, fileSystem);

            Mock.Get(mock)
                .Setup(x => x.Model)
                .Returns(() => new Model
                {
                    File = Path.GetRandomFileName()
                });

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
}