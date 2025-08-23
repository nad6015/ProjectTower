using System.Collections.Generic;
using Assets.DungeonGenerator;

namespace Assets.DungeonMaster
{
    public partial class DungeonMaster
    {
        public struct DungeonMasterConfiguration
        {
            public Dictionary<Difficulty, DungeonLayout> DungeonTemplates;
            public Dictionary<DungeonMission, List<DungeonRule>> Rulesets;
            public Dictionary<Difficulty, List<GameplayRule>> GameRulesets;
            public Dictionary<DungeonTheme, DungeonComponents> DungeonComponents;
            public Dictionary<DungeonMission, List<FlowPattern>> DungeonFlows;
        }
    }
}