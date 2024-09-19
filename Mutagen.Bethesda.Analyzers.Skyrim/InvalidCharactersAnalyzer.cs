using Mutagen.Bethesda.Analyzers.SDK.Topics;
namespace Mutagen.Bethesda.Analyzers.Skyrim;

public partial class InvalidCharactersAnalyzer
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

    public IEnumerable<TopicDefinition> Topics =>
    [
        InvalidCharactersBookText,
        InvalidCharactersDialogResponses,
        InvalidCharactersName
    ];
}
