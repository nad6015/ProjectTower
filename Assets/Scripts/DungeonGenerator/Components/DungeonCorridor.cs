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
        public Tuple<DungeonDoor, DungeonDoor> Doors { get; internal set; }

        private bool isHorizontal = false;
        private readonly List<GameObject> walls = new();
        private readonly List<GameObject> floors = new();

        /// <summary>
        /// Constructs a corridor.
        /// </summary>
        /// <param name="tilemap">the component used to construct the corridor</param>
        internal void Construct(DungeonTilemap tilemap, Vector3 minCorridorSize)
        {
            BoundsInt bounds = DungeonComponentUtils.BoundsToBoundsInt(Bounds);
            isHorizontal = Mathf.Approximately(minCorridorSize.z, Bounds.size.z);

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

            List<GameObject> doors = tilemap.DrawCorridorDoors(bounds, isHorizontal, transform);
            
            Doors = new(doors[0].GetComponent<DungeonDoor>(),
                        doors[1].GetComponent<DungeonDoor>());
        }

        public static DungeonCorridor Create(Bounds bounds, int id)
        {
            DungeonCorridor corridor = NewGameObjectWithComponent<DungeonCorridor>("Corridor " + id);
            corridor.Bounds = bounds;
            return corridor;
        }
    }
}