using Assets.DungeonGenerator.Components;
using System.Collections.Generic;

namespace Assets.DungeonGenerator
{
    /// <summary>
    /// TODO
    /// </summary>
    public abstract class Rule
    {
        public GameParameter GameParameter { get; }

        private List<ICondition> _conditions;
        private ValueRepresentation _value;
        private bool _conditionsMet;

        public Rule(GameParameter gameParameter, List<ICondition> conditions, ValueRepresentation value)
        {
            GameParameter = gameParameter;
            _conditions = conditions;
            _value = value;
        }

        public bool ConditionsMet(Dictionary<GameParameter, int> statistics)
        {
            _conditionsMet = false;
            int value = statistics[GameParameter];

            _conditions.ForEach(condition =>
            {
                _conditionsMet = condition.IsMet(value);
            });
            return _conditionsMet;
        }

        /// <summary>
        /// Gets the return value of the rule. Will return null until the rule's conditions are met.
        /// </summary>
        /// <returns>The rule's value</returns>
        public ValueRepresentation Value()
        {
            return _conditionsMet ? _value : null;
        }
    }
}