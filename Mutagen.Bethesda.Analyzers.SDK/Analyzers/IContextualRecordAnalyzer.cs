using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.SDK.Analyzers;

/// <summary>
/// An analyzer initiating from a specific record. <br />
/// Has the ability to analyze anything on the load order, from there. <br />
/// Must hand feed all relevant interesting fields for the records it intends to interact with
/// </summary>
/// <typeparam name="TMajor">The type of record being analyzed</typeparam>
public interface IContextualRecordAnalyzer<TMajor> : IAnalyzer
    where TMajor : IMajorRecordGetter
{
    /// <summary>
    /// Callback to execute the analyzer's logic
    /// </summary>
    /// <param name="param">Parameters for analysis</param>
    void AnalyzeRecord(ContextualRecordAnalyzerParams<TMajor> param);

    /// <summary>
    /// Callback to provide the fields that relate to an analyzer's analysis. <br />
    /// This is used to inform the engine of when the analyzer should be rerun.
    /// </summary>
    /// <returns>List of fields of interest to the analyzer</returns>
    IEnumerable<Func<TMajor, object?>> FieldsOfInterest();
}
