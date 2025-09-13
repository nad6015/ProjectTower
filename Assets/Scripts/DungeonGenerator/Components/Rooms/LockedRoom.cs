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
            DungeonNode lockedRoom = null;

            if (DungeonNode.LinkedNodes.Count > 2)
            {
                foreach (var node in DungeonNode.LinkedNodes)
                {
                    HashSet<DungeonNode> visitedNodes = new()
                {
                    node, DungeonNode
                };
                    DungeonNode fnode = FindPathTo(RoomType.End, node.LinkedNodes, visitedNodes);
                    if (fnode != null)
                    {
                        lockedRoom = node;
                        break;
                    }
                }
            }
            else
            {
                lockedRoom = DungeonNode.LinkedNodes[0];
            }

            DungeonCorridor corridor = null;
            foreach (var c in FindObjectsByType<DungeonCorridor>(FindObjectsSortMode.None))
            {
                if (c.Bounds.Intersects(Bounds) && c.Bounds.Intersects(lockedRoom.Bounds))
                {
                    corridor = c;
                    break;
                }
            }

            DungeonDoor door = corridor.Doors.Item1;

            this.door = door;

            base.Populate(dungeon);
        }

        public override void InstaniateContents(DungeonRepresentation dungeon)
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

        private DungeonNode FindPathTo(RoomType type, List<DungeonNode> connectedNodes, HashSet<DungeonNode> visitedNodes)
        {
            foreach (DungeonNode node in connectedNodes)
            {
                if (visitedNodes.Contains(node))
                {
                    continue;
                }
                visitedNodes.Add(node);
                if (node.Type == type)
                {
                    return node;
                }
                else
                {
                    return FindPathTo(type, node.LinkedNodes, visitedNodes);
                }
            }
            return null;
        }
    }
}