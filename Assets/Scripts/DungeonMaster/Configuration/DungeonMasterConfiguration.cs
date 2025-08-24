using System.Collections.Generic;
using Assets.DungeonGenerator;

namespace Assets.DungeonMaster
{
        public struct DungeonMasterConfiguration
        {
            public Dictionary<DungeonMission, List<DungeonRule>> Rulesets;
            public Dictionary<DungeonTheme, DungeonComponents> DungeonComponents;
            public Dictionary<DungeonMission, List<FlowPattern>> DungeonFlows;
    }
}