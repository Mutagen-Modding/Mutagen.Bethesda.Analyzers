using Mutagen.Bethesda.Plugins.Cache;

namespace Mutagen.Bethesda.Analyzers.Reporting;

public record struct ReportContextParameters(
    ILinkCache LinkCache);
