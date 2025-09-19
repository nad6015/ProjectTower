using UnityEngine;

namespace Assets.DungeonGenerator.Components
{
    public class DungeonEndRoom : DungeonRoom
    {
        internal override void Populate(DungeonRepresentation dungeon)
        {
            DungeonExit exit = dungeon.Components.exit;
            Vector3 pos = exit.transform.position + new Vector3(Bounds.center.x, 0, Bounds.center.z);
            
            Contents.Add(new(exit.gameObject, pos));
        }
    }
}