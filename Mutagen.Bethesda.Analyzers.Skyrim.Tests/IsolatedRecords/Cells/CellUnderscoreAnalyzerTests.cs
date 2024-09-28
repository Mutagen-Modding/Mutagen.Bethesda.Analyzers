using Mutagen.Bethesda.Analyzers.Skyrim.Record.Cell;
using Mutagen.Bethesda.Analyzers.Testing.Frameworks;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Testing.AutoData;
using Xunit;

namespace Mutagen.Bethesda.Analyzers.Skyrim.Tests.IsolatedRecords.Cells;

public class CellUnderscoreAnalyzerTests
{
    [Theory, MutagenModAutoData]
    public void Typical(
        IsolatedRecordTestFixture<CellUnderscoreAnalyzer, Cell, ICellGetter> fixture)
    {
        fixture.Run(
            prepForError: cell => cell.EditorID = "Bad_Editor_Id",
            prepForFix: cell => cell.EditorID = "GoodEditorId",
            CellUnderscoreAnalyzer.CellUnderscoreWrong);
    }
}
