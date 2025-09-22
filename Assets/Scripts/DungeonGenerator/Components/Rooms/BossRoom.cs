using Assets.Combat;
using UnityEngine;
using static Assets.Utilities.GameObjectUtilities;

namespace Assets.DungeonGenerator.Components.Rooms
{
    public class BossRoom : DungeonRoom
    {
        private DungeonDoor _door;
        private Enemy _boss;
        internal override void Populate(DungeonRepresentation dungeon)
        {
            PlaceProps(dungeon);
            Contents.Add(new(dungeon.Components.boss.gameObject, Bounds.center));
            _door = FindClosestDoor(1);
        }

        internal override void InstaniateContents(DungeonRepresentation dungeon)
        {
            base.InstaniateContents(dungeon);
            _boss = FindComponentByTag<Enemy>("Boss");
            _boss.OnDefeat += BossDefeated;
            _door.LockDoor();
        }

        private void BossDefeated(Fighter obj)
        {
            _door.Unlock();
        }
    }
}