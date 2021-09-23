using Mutagen.Bethesda.Analyzers.Reporting;
using Mutagen.Bethesda.Plugins.Records;

namespace Mutagen.Bethesda.Analyzers.Drivers
{
    public interface IModDriver
    {
        bool Applicable { get; }
        void Drive(IModGetter modGetter, IReportDropbox dropbox);
    }
}
