using Assets.DungeonGenerator.Components;

namespace Assets.DungeonGenerator
{
    /// <summary>
    /// Interface for all dungeon algorithms. Used to ensure all algorithms have a consistent contract.
    /// </summary>
    internal interface IDungeonAlgorithm
    {
        /// <summary>
        /// Generates a dungeon. Depending on the duties of the algorithm, the dungeon may be modified
        /// during the generation process.
        /// </summary>
        /// <param name="dungeon">the representation to use as a blueprint</param>
        void GenerateDungeon(DungeonRepresentation dungeon);

        /// <summary>
        /// Clears an algorithm of any state used to generate a dungeon.
        /// </summary>
        void ClearDungeon();
    }
}