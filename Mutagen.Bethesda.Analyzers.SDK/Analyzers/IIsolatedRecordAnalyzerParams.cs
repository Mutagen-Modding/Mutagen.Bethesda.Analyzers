using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.SDK.Analyzers
{
    public interface IIsolatedRecordAnalyzerParams<out TMajor>
        where TMajor : IMajorRecordGetter
    {
        public TMajor Record { get; }
    }

    public record IsolatedRecordAnalyzerParams<TMajor>(TMajor Record) : IIsolatedRecordAnalyzerParams<TMajor>
        where TMajor : IMajorRecordGetter;
}
