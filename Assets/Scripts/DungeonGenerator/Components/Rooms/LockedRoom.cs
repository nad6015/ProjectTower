using Assets.Interactables;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.DungeonGenerator.Components
{
    public class LockedRoom : DungeonRoom
    {
        private DungeonDoor door;

        internal override void Populate(DungeonRepresentation dungeon)
        {

            door = FindClosestDoor();

            base.Populate(dungeon);
        }

        internal override void InstaniateContents(DungeonRepresentation dungeon)
        {
            var dungeonRooms = dungeon.GetConstructedDungeon().DungeonRooms;

            PickupItem keyPickup = Instantiate(dungeon.Components.doorKey);

            int index = Random.Range(0, dungeonRooms.IndexOf(this));
            DungeonRoom room = dungeonRooms[index];
            keyPickup.transform.SetParent(room.transform);
            Bounds safeBounds = new(room.Bounds.center, room.Bounds.size / 2f);
            keyPickup.transform.position = PointUtils.RandomPointWithinBounds(safeBounds);

            door.LockDoor(keyPickup.Pickup.GetComponent<DoorKey>());
        }
    }
}