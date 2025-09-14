using UnityEngine;

namespace Assets.DungeonMaster
{
    /// <summary>
    /// Represents a condition within a DungeonMasterRule.
    /// </summary>
    public class GreaterThanCondition : ICondition
    {
        private readonly int _operand;

        public GreaterThanCondition(int operand)
        {
            _operand = operand;
        }

        public bool IsMet(int value)
        {
            return value > _operand;
        }
    }
}