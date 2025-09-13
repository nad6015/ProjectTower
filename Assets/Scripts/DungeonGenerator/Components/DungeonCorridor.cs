using Assets.DungeonGenerator.Components;
using Assets.DungeonGenerator.Components.Tiles;
using System;
using System.Collections.Generic;
using UnityEngine;

using static Assets.Utilities.GameObjectUtilities;

namespace Assets.DungeonGenerator
{
    public class DungeonCorridor : MonoBehaviour
    {
        public Bounds Bounds { get; private set; }
        public Tuple<DungeonNode, DungeonNode> ConnectedRooms { get; private set; }
        public DungeonAxis Axis { get; private set; }
        public Tuple<DungeonDoor, DungeonDoor> Doors { get; private set; }

        private readonly List<GameObject> walls = new();
        private readonly List<GameObject> floors = new();

        /// <summary>
        /// Constructs a corridor.
        /// </summary>
        /// <param name="tilemap">the component used to construct the corridor</param>
        internal void Construct(DungeonTilemap tilemap)
        {
            BoundsInt bounds = DungeonComponentUtils.BoundsToBoundsInt(Bounds);
            bool isHorizontal = Axis == DungeonAxis.Horizontal;

            transform.position = bounds.min;

            // Create an empty game object to contain the floor tiles
            GameObject floor = new("Floor");
            floor.transform.SetParent(transform);

            // Place the floor
            floors.AddRange(DungeonComponentUtils.DrawFloor(tilemap, bounds, floor.transform));

            // Place the walls depending on the corridor's axis

            if (isHorizontal)
            {
                walls.AddRange(DungeonComponentUtils.DrawTopAndBottomWalls(tilemap, bounds, transform));
            }
            else
            {
                walls.AddRange(DungeonComponentUtils.DrawLeftAndRightWalls(tilemap, bounds, transform));
            }

            var tempDoors = tilemap.DrawCorridorDoors(bounds, isHorizontal, transform);
            Doors = new(tempDoors.Item1.GetComponent<DungeonDoor>(), tempDoors.Item2.GetComponent<DungeonDoor>());
        }

        public static DungeonCorridor Create(DungeonNodeLink link, int id)
        {
            DungeonCorridor corridor = NewGameObjectWithComponent<DungeonCorridor>("Corridor " + id);
            corridor.Bounds = link.Bounds;
            corridor.ConnectedRooms = link.ConnectedRooms;
            corridor.Axis = link.Axis;
            return corridor;
        }
    }
}