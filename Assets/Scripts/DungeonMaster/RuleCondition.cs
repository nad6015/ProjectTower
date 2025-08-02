using System.Collections.Generic;
using UnityEngine;

namespace Assets.DungeonGenerator
{
    /// <summary>
    /// Represents a condition within a DungeonMasterRule.
    /// </summary>
    public class RuleCondition
    {
        private readonly string _operand;
        private readonly string _operator;

        public RuleCondition(string @operator, string operand)
        {
            _operand = operand;
            _operator = @operator;
        }

        public bool IsMet(float value)
        {

            float operand = float.Parse(_operand);
            switch (_operator)
            {
                case ">": return value > operand;
                case "<": return value < operand;
            }

            return false;
        }
    }
}