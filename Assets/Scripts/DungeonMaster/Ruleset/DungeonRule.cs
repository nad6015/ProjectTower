using Assets.DungeonGenerator;
using Assets.DungeonGenerator.Components;
using System.Collections.Generic;

namespace Assets.DungeonMaster
{
    public class DungeonRule : Rule
    {
        public DungeonParameter Parameter { get; }

        public DungeonRule(string id, DungeonParameter parameter, GameParameter gameParameter, List<ICondition> conditions, ValueRepresentation value) 
            : base(id, gameParameter, conditions, value)
        {
            Parameter = parameter;
        }
    }
}