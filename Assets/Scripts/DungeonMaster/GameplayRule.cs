using Assets.DungeonGenerator.Components;
using System.Collections.Generic;

namespace Assets.DungeonGenerator
{
    public class GameplayRule : Rule
    {
        public GameplayParameter Parameter { get; }

        public GameplayRule(GameplayParameter parameter, GameParameter gameParameter, List<ICondition> conditions, ValueRepresentation value) :
            base(gameParameter, conditions, value)
        {
            Parameter = parameter;
        }
    }
}