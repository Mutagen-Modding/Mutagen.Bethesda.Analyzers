﻿using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Skyrim;
using Noggog;

namespace Mutagen.Bethesda.Analyzers.Skyrim;

public class CellUnderscoreAnalyzer : IIsolatedRecordAnalyzer<ICellGetter>
{
    public static readonly TopicDefinition<string?> CellUnderscoreWrong = MutagenTopicBuilder.FromDiscussion(
            33,
            "EDID (Editor ID) for Cells should not have underscores in them",
            Severity.Error)
        .WithFormatting<string?>("Cell editor ids must not have underscores");

    public RecordAnalyzerResult? AnalyzeRecord(IsolatedRecordAnalyzerParams<ICellGetter> param)
    {
        if ((!param.Record.EditorID?.Contains("_")) ?? true)
        {
            return null;
        }
        return new RecordAnalyzerResult(
            RecordTopic.Create(
                obj: param.Record,
                formattedTopicDefinition: CellUnderscoreWrong.Format(param.Record.EditorID),
                memberExpression: x => x.EditorID
            )
        );
    }

    public IEnumerable<TopicDefinition> Topics => CellUnderscoreWrong.AsEnumerable();
}
