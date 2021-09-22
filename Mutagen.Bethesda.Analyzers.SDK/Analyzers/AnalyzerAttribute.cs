using System;
using JetBrains.Annotations;

namespace Mutagen.Bethesda.Analyzers.SDK.Analyzers
{
    [AttributeUsage(AttributeTargets.Class)]
    [MeansImplicitUse(ImplicitUseTargetFlags.WithInheritors)]
    [PublicAPI]
    public class AnalyzerAttribute : Attribute { }
}
