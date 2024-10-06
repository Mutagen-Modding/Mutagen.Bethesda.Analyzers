﻿using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;

namespace Mutagen.Bethesda.Analyzers.Skyrim;

public partial class InvalidCharactersAnalyzer : IIsolatedRecordAnalyzer<IBookGetter>
{
    public static readonly TopicDefinition<IEnumerable<string>> InvalidCharactersBookText = MutagenTopicBuilder.DevelopmentTopic(
            "Book Text Contains Invalid Characters",
            Severity.Warning)
        .WithFormatting<IEnumerable<string>>("Book text contains invalid characters: {0}");

    public void AnalyzeRecord(IsolatedRecordAnalyzerParams<IBookGetter> param)
    {
        var book = param.Record;
        if (book.BookText.String is null) return;

        var invalidStrings = InvalidStrings.Where(invalidString => book.BookText.String.Contains(invalidString.Key)).ToList();
        if (invalidStrings.Count == 0) return;

        param.AddTopic(
            InvalidCharactersBookText.Format(invalidStrings.Select(x => x.Key)));
    }

    IEnumerable<Func<IBookGetter, object?>> IIsolatedRecordAnalyzer<IBookGetter>.FieldsOfInterest()
    {
        yield return x => x.BookText;
    }
}
