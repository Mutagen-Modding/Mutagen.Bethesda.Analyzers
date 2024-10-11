namespace Mutagen.Bethesda.Analyzers.Skyrim;

public static class InvalidCharactersAnalyzerUtil
{
    public static Dictionary<string, string> InvalidStrings { get; } = new()
    {
        { "’", "'" },
        { "`", "'" },
        { "”", "\"" },
        { "“", "\"" },
        { "…", "..." },
        { "—", "-" },
    };

    public static Dictionary<string, string> InvalidStringsOneLiner { get; } = InvalidStrings
        .Concat(new Dictionary<string, string> { { "\r", "" }, { "\n", "" }, { "  ", " " } })
        .ToDictionary();
}
