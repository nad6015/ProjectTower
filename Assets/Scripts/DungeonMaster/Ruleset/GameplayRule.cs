using Assets.DungeonGenerator.Components;
using System.Collections.Generic;

namespace Assets.DungeonMaster
{
    public class GameplayRule : Rule
    {
        public GameplayParameter Parameter { get; }

        public GameplayRule(string id, GameplayParameter parameter, GameParameter gameParameter, List<ICondition> conditions, ValueRepresentation value) :
            base(id, gameParameter, conditions, value)
        {
            Parameter = parameter;
        }
    }
}