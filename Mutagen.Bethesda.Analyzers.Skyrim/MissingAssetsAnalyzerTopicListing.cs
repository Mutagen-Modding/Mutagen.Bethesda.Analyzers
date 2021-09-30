using System.Collections.Generic;
using Mutagen.Bethesda.Analyzers.SDK.Topics;

namespace Mutagen.Bethesda.Analyzers.Skyrim
{
    // Eventually can populate this via Source Generators
    public partial class MissingAssetsAnalyzer
    {
        public IEnumerable<ITopicDefinition> Topics => GetTopics();

        private IEnumerable<ITopicDefinition> GetTopics()
        {
            yield return MissingWeaponModel;
            yield return MissingTextureInTextureSet;
            yield return MissingStaticModel;
            yield return MissingHeadPartModel;
            yield return MissingHeadPartFile;
            yield return MissingArmorAddonWorldModel;
            yield return MissingArmorAddonFirstPersonModel;
            yield return MissingArmorModel;
        }
    }
}
