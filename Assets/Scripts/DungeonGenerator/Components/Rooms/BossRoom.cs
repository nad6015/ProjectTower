using UnityEngine;

namespace Assets.DungeonGenerator.Components.Rooms
{
    public class BossRoom : DungeonRoom
    {
        internal override void Populate(DungeonRepresentation dungeon)
        {
            PlaceProps(dungeon);
            PlaceProps(dungeon);
            Contents.Add(new(dungeon.Components.boss.gameObject, Bounds.center));
        }
    }
}