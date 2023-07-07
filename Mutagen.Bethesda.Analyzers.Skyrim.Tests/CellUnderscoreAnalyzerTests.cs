using Mutagen.Bethesda.Analyzers.TestingUtils;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Testing.AutoData;
using Xunit;

namespace Mutagen.Bethesda.Analyzers.Skyrim.Tests;

public class CellUnderscoreAnalyzerTests
{
    [Theory, MutagenModAutoData]
    public void TestCellUnderscoreAnalyzer(
        CellUnderscoreAnalyzer sut
    )
    {
        // # Given
        Cell cell = new Cell(FormKey.Null, SkyrimRelease.SkyrimSE);
        cell.EditorID = "GoodEditorId";
        var givenCellGetter = cell.AsIsolatedParams<ICellGetter>();
        // # When
        var result = sut.AnalyzeRecord(givenCellGetter);
        // # Then
        Assert.Equal(null, result);
    }

    [Theory, MutagenModAutoData]
    public void TestCellUnderscoreAnalyzerGivenBadEditorId(
        CellUnderscoreAnalyzer sut
    )
    {
        // # Given
        Cell cell = new Cell(FormKey.Null, SkyrimRelease.SkyrimSE);
        cell.EditorID = "Bad_Editor_Id";
        var givenCellGetter = cell.AsIsolatedParams<ICellGetter>();
        // # When
        var result = sut.AnalyzeRecord(givenCellGetter);
        // # Then
        Assert.NotEqual(null, result);
    }
}
