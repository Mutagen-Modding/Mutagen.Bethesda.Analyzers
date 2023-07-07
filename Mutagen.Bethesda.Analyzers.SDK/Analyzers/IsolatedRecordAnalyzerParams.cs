namespace Mutagen.Bethesda.Analyzers.SDK.Analyzers;

public readonly struct IsolatedRecordAnalyzerParams<TMajor>
{
    public readonly TMajor Record;

    public IsolatedRecordAnalyzerParams(TMajor @record)
    {
        Record = record;
    }
}

public readonly struct IsolatedRecordFixerParams<TMajor>
{
    public readonly TMajor Record;

    public IsolatedRecordFixerParams(TMajor @record)
    {
        Record = record;
    }
}
