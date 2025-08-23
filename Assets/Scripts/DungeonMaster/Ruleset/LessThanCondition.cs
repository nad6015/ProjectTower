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
            return value < _operand;
        }
    }
}