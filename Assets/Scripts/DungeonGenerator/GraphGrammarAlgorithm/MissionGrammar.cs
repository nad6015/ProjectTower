using Assets.DungeonGenerator.Components;
using Assets.Scripts.DungeonGenerator.Components;

namespace Assets.DungeonGenerator
{
    public class MissionGrammar : IDungeonAlgorithm
    {
        private Dungeon _dungeon;
        private Graph<DungeonRoom> _dungeonRooms = new();

        /// <summary>
        /// Generates a representation of dungeon using the given parameters.
        /// </summary>
        /// <param name="parameters"></param>
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