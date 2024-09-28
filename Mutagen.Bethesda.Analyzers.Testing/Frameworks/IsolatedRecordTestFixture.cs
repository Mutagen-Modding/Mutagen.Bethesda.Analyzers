using AutoFixture;
using FluentAssertions;
using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Testing.Frameworks;

public class IsolatedRecordTestFixture<TAnalyzer, TMajor, TMajorGetter>
    where TMajor : IMajorRecord, TMajorGetter
    where TMajorGetter : IMajorRecordGetter
    where TAnalyzer : IIsolatedRecordAnalyzer<TMajorGetter>
{
    private readonly IFixture _fixture;
    public TAnalyzer Sut { get; }

    public IsolatedRecordTestFixture(TAnalyzer sut, IFixture fixture)
    {
        _fixture = fixture;
        Sut = sut;
    }

    public void Run(
        Action<TMajor> prepForError,
        Action<TMajor> prepForFix,
        params TopicDefinition[] expectedTopics)
    {
        var rec = _fixture.Create<TMajor>();
        prepForError(rec);
        var param = new IsolatedRecordAnalyzerParams<TMajorGetter>(rec);

        var errorResults = Sut.AnalyzeRecord(param)?.Topics ?? [];
        errorResults.Select(x => x.TopicDefinition.Id)
            .Should().Equal(expectedTopics.Select(x => x.Id));

        prepForFix(rec);

        // ToDo
        // Eventually test that fixrec triggers a rerun in the engine properly

        var fixResults = Sut.AnalyzeRecord(param)?.Topics ?? [];
        fixResults.Should().BeEmpty();
    }

    public void RunShouldBeNoError(
        Action<TMajor> prep)
    {
        var rec = _fixture.Create<TMajor>();
        prep(rec);
        var param = new IsolatedRecordAnalyzerParams<TMajorGetter>(rec);
        var results = Sut.AnalyzeRecord(param)?.Topics ?? [];
        results.Should().BeEmpty();
    }
}
