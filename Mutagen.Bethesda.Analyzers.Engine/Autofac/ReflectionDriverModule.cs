using System.Linq;
using Autofac;
using Loqui;
using Mutagen.Bethesda.Analyzers.Drivers;
using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Noggog;

namespace Mutagen.Bethesda.Analyzers.Autofac
{
    public class ReflectionDriverModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            foreach (var analyzerType in TypeExt.GetInheritingFromGenericInterface(
                    typeof(IRecordAnalyzer<>),
                    loadAssemblies: true)
                .Select(x => x.Key.GetGenericArguments()[0])
                .Select(x => LoquiRegistration.GetRegister(x).GetterType)
                .Distinct())
            {
                builder.RegisterType(typeof(ByTypeDriver<>).MakeGenericType(analyzerType))
                    .As<IDriver>();
            }
        }
    }
}
