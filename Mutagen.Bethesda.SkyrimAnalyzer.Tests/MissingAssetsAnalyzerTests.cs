using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.Skyrim;
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
            Assert.Collection(
                result.Errors,
                x => Assert.Equal(MissingAssetsAnalyzer.MissingFemaleArmorModel, x.Message),
                x => Assert.Equal(MissingAssetsAnalyzer.MissingMaleArmorModel, x.Message)
                );
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
