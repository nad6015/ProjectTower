using Assets.DungeonGenerator.Components;
using Assets.DungeonGenerator.Components.Tiles;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DungeonGenerator
{
    public class DungeonCorridor : MonoBehaviour
    {
        public Bounds Bounds { get; private set; }
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
            isHorizontal = minCorridorSize.z == Bounds.size.z;

            transform.position = bounds.min;

            // Place the floor
            floors.AddRange(DungeonComponentUtils.DrawFloor(tilemap, bounds, transform));

            // Place the walls depending on the corridor's axis

            if (isHorizontal)
            {
                walls.AddRange(DungeonComponentUtils.DrawTopAndBottomWalls(tilemap, bounds, transform));
            }
            else
            {
                walls.AddRange(DungeonComponentUtils.DrawLeftAndRightWalls(tilemap, bounds, transform));
            }
        }

        public static DungeonCorridor Create(Bounds bounds, int id)
        {
            // Create gameobject code referenced from  - https://discussions.unity.com/t/how-do-you-create-an-empty-gameobject-in-code-and-add-it-to-the-scene/86380/4
            GameObject gameObj = new("Corridor " + id);
            DungeonCorridor corridor = gameObj.AddComponent<DungeonCorridor>();
            corridor.Bounds = bounds;
            return corridor;
        }
    }
}