using Mutagen.Bethesda.Plugins.Binary.Headers;

namespace Mutagen.Bethesda.Analyzers.Drivers.RecordFrame;

public interface IContextualRecordFrameAnalyzerDriver : IRecordFrameAnalyzerBundle
{
    Task Drive(ContextualDriverParams driverParams, MajorRecordFrame frame);
}
