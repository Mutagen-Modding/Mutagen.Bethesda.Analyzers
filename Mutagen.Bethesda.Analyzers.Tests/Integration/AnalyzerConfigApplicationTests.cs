using Autofac;
using FluentAssertions;
using Mutagen.Bethesda.Analyzers.Config;
using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Mutagen.Bethesda.Analyzers.SDK.Topics;
using NSubstitute;
using Xunit;

namespace Mutagen.Bethesda.Analyzers.Tests.Integration;

public class AnalyzerConfigApplicationTests
{
    [Theory]
    [AnalyzerAutoData]
    public void NoReturnActsNormally(IContextualAnalyzer testAnalyzer)
    {
        var dropoff = Utility.RunTest(builder =>
        {
            testAnalyzer.Analyze(default).ReturnsForAnyArgs(default(ContextualAnalyzerResult?));
            builder.RegisterInstance(testAnalyzer).AsSelf();
        });
        dropoff.Reports.Should().HaveCount(0);
    }

    [Fact]
    public void PassesDesiredSeverity()
    {
        var dropoff = Utility.RunTest(builder =>
        {
            builder.RegisterInstance(Utility.WarningAnalyzer).AsImplementedInterfaces();
        });
        dropoff.Reports.Should().HaveCount(1);
    }

    [Fact]
    public void FiltersUndesiredSeverity()
    {
        var dropoff = Utility.RunTest(builder =>
        {
            builder.RegisterInstance(Utility.SuggestionAnalyzer).AsImplementedInterfaces();
            var minSev = Substitute.For<IMinimumSeverityConfiguration>();
            minSev.MinimumSeverity.Returns(Severity.Warning);
            builder.RegisterInstance(minSev).As<IMinimumSeverityConfiguration>();
        });
        dropoff.Reports.Should().HaveCount(0);
    }

    [Fact]
    public void AdjustsTopicSeverity()
    {
        var dropoff = Utility.RunTest(builder =>
        {
            builder.RegisterInstance(Utility.WarningAnalyzer).AsImplementedInterfaces();
            var minSev = Substitute.For<IMinimumSeverityConfiguration>();
            minSev.MinimumSeverity.Returns(Severity.Warning);
            builder.RegisterInstance(minSev).As<IMinimumSeverityConfiguration>();
            var sevLookup = Substitute.For<ISeverityLookup>();
            sevLookup.LookupSeverity(Utility.Warning).Returns(Severity.Suggestion);
            builder.RegisterInstance(sevLookup).As<ISeverityLookup>();
        });
        dropoff.Reports.Should().HaveCount(0);
    }
}
