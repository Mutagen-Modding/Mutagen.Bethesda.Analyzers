using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.SDK.Analyzers;

/// <summary>
/// An analyzer initiating from a specific record, but allowed to resolve other declared record types
/// </summary>
/// /// <typeparam name="TDriver">The type of record being analyzed</typeparam>
/// <typeparam name="TLookup1">A type of record allowed to be resolved by the link cache</typeparam>
public interface IIsolatedRecordsAnalyzer<TDriver, TLookup1> : IAnalyzer
    where TDriver : class, IMajorRecordGetter
    where TLookup1 : class, IMajorRecordGetter
{
    /// <summary>
    /// Callback to execute the analyzer's logic
    /// </summary>
    /// <param name="param">Parameters for analysis</param>
    void AnalyzeRecord(IsolatedRecordsAnalyzerParams<TDriver, TLookup1> param);

    /// <summary>
    /// Callback to provide the fields that relate to an analyzer's analysis. <br />
    /// This is used to inform the engine of when the analyzer should be rerun.
    /// </summary>
    /// <returns>List of fields of interest to the analyzer</returns>
    IEnumerable<Func<TDriver, object?>> DriverFieldsOfInterest();

    /// <summary>
    /// Callback to provide the fields that relate to an analyzer's analysis. <br />
    /// This is used to inform the engine of when the analyzer should be rerun.
    /// </summary>
    /// <returns>List of fields of interest to the analyzer</returns>
    IEnumerable<Func<TLookup1, object?>> LookupFieldsOfInterest();
}

/// <summary>
/// An analyzer initiating from a specific record, but allowed to resolve other declared record types
/// </summary>
/// <typeparam name="TDriver">The type of record being analyzed</typeparam>
/// <typeparam name="TLookup1">A type of record allowed to be resolved by the link cache</typeparam>
/// <typeparam name="TLookup2">A type of record allowed to be resolved by the link cache</typeparam>
public interface IIsolatedRecordsAnalyzer<TDriver, TLookup1, TLookup2> : IAnalyzer
    where TDriver : class, IMajorRecordGetter
    where TLookup1 : class, IMajorRecordGetter
    where TLookup2 : class, IMajorRecordGetter
{
    /// <summary>
    /// Callback to execute the analyzer's logic
    /// </summary>
    /// <param name="param">Parameters for analysis</param>
    void AnalyzeRecord(ContextualRecordAnalyzerParams<TDriver> param);

    /// <summary>
    /// Callback to provide the fields that relate to an analyzer's analysis. <br />
    /// This is used to inform the engine of when the analyzer should be rerun.
    /// </summary>
    /// <returns>List of fields of interest to the analyzer</returns>
    IEnumerable<Func<TDriver, object?>> DriverFieldsOfInterest();

    /// <summary>
    /// Callback to provide the fields that relate to an analyzer's analysis. <br />
    /// This is used to inform the engine of when the analyzer should be rerun.
    /// </summary>
    /// <returns>List of fields of interest to the analyzer</returns>
    IEnumerable<Func<TLookup1, object?>> LookupFieldsOfInterest();

    /// <summary>
    /// Callback to provide the fields that relate to an analyzer's analysis. <br />
    /// This is used to inform the engine of when the analyzer should be rerun.
    /// </summary>
    /// <returns>List of fields of interest to the analyzer</returns>
    IEnumerable<Func<TLookup2, object?>> Lookup2FieldsOfInterest();
}
