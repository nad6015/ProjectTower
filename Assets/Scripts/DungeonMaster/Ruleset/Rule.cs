using Assets.DungeonGenerator;
using Assets.DungeonGenerator.Components;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DungeonMaster
{
    /// <summary>
    /// A generic rule class. Used to store the conditions and return value of a rule.
    /// </summary>
    public abstract class Rule
    {
        public GameParameter GameParameter { get; }
        public string Id { get; }
        private List<ICondition> _conditions;
        private ValueRepresentation _value;
        private bool _conditionsMet;

        public Rule(string id, GameParameter gameParameter, List<ICondition> conditions, ValueRepresentation value)
        {
            Id = id;
            GameParameter = gameParameter;
            _conditions = conditions;
            _value = value;
        }


        /// <summary>
        /// Checks if the rule's conditions are met.
        /// </summary>
        /// <param name="statistics"></param>
        /// <returns></returns>
        public bool ConditionsMet(Dictionary<GameParameter, int> statistics)
        {
            _conditionsMet = false;
            if (!statistics.ContainsKey(GameParameter))
            {
                return false;
            }

            int value = statistics[GameParameter];
            Debug.Log(Id);
            foreach (var condition in _conditions)
            {
                if (!condition.IsMet(value))
                {
                    Debug.Log(Id+ " Condition is not met");
                    Debug.Log(value);

                    _conditionsMet = false;
                    break;
                }
                else
                {
                    Debug.Log(Id + "Condition is met");
                    Debug.Log(value);
                    _conditionsMet = true;
                }
            };
            Debug.Log(Id+ " rule has " + (_conditionsMet ? "all conditions met" : "no or partial conditions met"));
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