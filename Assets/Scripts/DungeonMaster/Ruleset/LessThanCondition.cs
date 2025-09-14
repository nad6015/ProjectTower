using UnityEngine;

namespace Assets.DungeonMaster
{
    /// <summary>
    /// Represents a condition within a DungeonMasterRule.
    /// </summary>
    public class LessThanCondition : ICondition
    {
        private readonly int _operand;

        public LessThanCondition(int operand)
        {
            _operand = operand;
        }

        public bool IsMet(int value)
        {
            Debug.Log("value < _operand = " + (value < _operand));
            Debug.Log("value = " + value);
            Debug.Log("_operand = " + _operand);

            return value < _operand;
        }
    }
}