using UnityEngine;

namespace Assets.DungeonGenerator.Components.Rooms
{
    public class TreasureRoom : DungeonRoom
    {
        internal override void Populate(DungeonRepresentation dungeon)
        {
            Contents.Add(new(dungeon.Components.chest.gameObject, Bounds.center));
        }
    }
}