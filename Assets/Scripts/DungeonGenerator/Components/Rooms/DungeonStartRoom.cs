using UnityEngine;

namespace Assets.DungeonGenerator.Components
{
    public class DungeonStartRoom : DungeonRoom
    {
        internal override void Populate(DungeonRepresentation dungeon)
        {
            base.Populate(dungeon);
            SpawnPoint startingPoint = dungeon.Components.startingPoint;
            Contents.Add(new(startingPoint.gameObject, startingPoint.transform.position + Bounds.center));
        }
    }
}