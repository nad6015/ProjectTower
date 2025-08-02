using System.Collections.Generic;
using UnityEngine;

namespace Assets.DungeonGenerator
{
    public class DungeonCorridor : MonoBehaviour
    {
        public Bounds Bounds { get; private set; }

        private readonly List<GameObject> walls = new();

        /// <summary>
        /// Constructs a corridor.
        /// </summary>
        /// <param name="components">the component used to construct the corridor</param>
        internal void Construct(DungeonComponents components, Vector3 minCorridorSize)
        {
            GameObject wallAsset = components.corridorWall;
            GameObject floorAsset = components.floorTile;

            float width = Bounds.size.x;
            float height = Bounds.size.z;

            float minX = Bounds.min.x;
            float minZ = Bounds.min.z;

            float maxX = Bounds.max.x;
            float maxZ = Bounds.max.z;

            bool isHorizontal = minCorridorSize.y == height;

            float angle = isHorizontal ? 0: 90f;
            float count  = isHorizontal ? width : height;

            transform.position = Bounds.min;

            GameObject floor = Instantiate(floorAsset, new(minX, 0, minZ), Quaternion.identity, transform);
            floor.name = "Floor";
            floor.transform.localScale = PointUtils.Vec2ToVec3(Bounds.size, 0.5f);

            // Place the walls
            for (int i = 0; i < count; i++)
            {
                float x = isHorizontal ? minX + i : minX;
                float z = isHorizontal ? minZ : minZ + i;

                GameObject wall = Instantiate(wallAsset, new Vector3(x, 0, z), Quaternion.identity, transform);
                wall.name = "Wall Bottom";
                wall.transform.GetChild(0).Rotate(Vector3.up, angle);
                walls.Add(wall);

                x = isHorizontal ? minX + i : maxX;
                z = isHorizontal ? maxZ : minZ + i;

                wall = Instantiate(wallAsset, new Vector3(x, 0, z), Quaternion.identity, transform);
                wall.name = "Wall Top";
                wall.transform.GetChild(0).Rotate(Vector3.up, angle);
                walls.Add(wall);
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

        public void Modify(Bounds bounds)
        {
            foreach (var wall in walls)
            {
                Vector3 wallPos = wall.transform.position;
                if ((wallPos.x > bounds.min.x && wallPos.x < bounds.max.x) && (wallPos.z > bounds.min.y && wallPos.z < bounds.max.y))
                {
                    wall.SetActive(false);
                }
            }
        }
    }
}