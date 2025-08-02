using System.Collections.Generic;
using UnityEngine;

namespace Assets.DungeonGenerator
{
    /// <summary>
    /// TODO
    /// </summary>
    public class DungeonMasterRule
    {
        public string Id { get; private set; }
        public string ParamName { get; private set; }

        private List<RuleCondition> _conditions;
        private RuleValue _value;
        private bool _conditionsMet;

        public DungeonMasterRule(string id, string parameter, List<RuleCondition> conditions, RuleValue value)
        {
            Id = id;
            ParamName = parameter;
            _conditions = conditions;
            _value = value;
        }

        public bool ConditionsMet(float val)
        {
            _conditionsMet = false;

            _conditions.ForEach(condition =>
            {
                _conditionsMet = condition.IsMet(val);
            });
            return _conditionsMet;
        }

        /// <summary>
        /// Gets the return value of the rule. Will return null until the rule's conditions are met.
        /// </summary>
        /// <returns>The rule's value</returns>
        public RuleValue RuleValue()
        {
            return _conditionsMet ? _value : null;
        }
    }
}