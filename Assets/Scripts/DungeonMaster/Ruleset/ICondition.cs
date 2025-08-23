namespace Assets.DungeonMaster
{
    /// <summary>
    /// Common interface for rule conditions.
    /// </summary>
    public interface ICondition
    {
        public bool IsMet(int value);
    }
}