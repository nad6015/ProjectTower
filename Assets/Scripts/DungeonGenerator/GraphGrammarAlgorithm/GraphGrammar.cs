using Assets.DungeonGenerator.Components;
using Assets.Scripts.DungeonGenerator.Components;

namespace Assets.DungeonGenerator
{
    public class GraphGrammar : IDungeonAlgorithm
    {
        private Dungeon _dungeon;
        private Graph<DungeonRoom> _dungeonRooms = new();

        /// <summary>
        /// <inheritdoc/>
        /// The algorithm for this generation is as such:
        /// 1. Create a graph representation of a dungeon using a base template.
        /// 2. Using the rules within the dungeon parameters, replace any graph nodes that match a rule's pattern with the rule's value.
        /// 3. Repeat step 2 until no matches are left or desired number of rooms are met.
        /// 4. Modify the dungeon to hold this information.
        /// </summary>
        /// <param name="dungeon">the dungoen instance</param>
        public void GenerateDungeon(Dungeon dungeon)
        {
            // TODO
        }
        
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void ClearDungeon()
        {
            _dungeonRooms.RemoveAll();
        }
    }
}