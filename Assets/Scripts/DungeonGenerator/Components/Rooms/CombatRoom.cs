using UnityEngine;

namespace Assets.DungeonGenerator.Components
{
    public class CombatRoom : DungeonRoom
    {
        internal override void Populate(DungeonRepresentation dungeon)
        {
            PlaceProps(dungeon);
            SpawnEnemies(dungeon);
            // TODO: chance of combat room being locked
        }
    }
}