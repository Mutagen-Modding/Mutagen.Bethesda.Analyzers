using Mutagen.Bethesda.Plugins.Cache;

namespace Mutagen.Bethesda.Analyzers.SDK.Drops;

public record struct ReportContextParameters(
    ILinkCache LinkCache);
