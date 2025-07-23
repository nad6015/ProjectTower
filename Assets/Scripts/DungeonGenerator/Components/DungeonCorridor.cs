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
            GameObject wallAsset = components.corridorTile;
            GameObject floorAsset = components.floorTile;

            float width = Bounds.size.x;
            float height = Bounds.size.z;

            float minX = Bounds.min.x;
            float minZ = Bounds.min.z;

            float maxX = Bounds.max.x;
            float maxZ = Bounds.max.z;

            bool isHorizontal = minCorridorSize.y == height;

            transform.position = Bounds.min;

            GameObject floor = Instantiate(floorAsset, transform);
            floor.transform.localScale = PointUtils.Vec2ToVec3(Bounds.size, 0.5f);

            if (isHorizontal)
            {
                // Place the top and bottoms wall
                for (int i = 0; i < width; i++)
                {
                    float wallX = minX + i;

                    GameObject wall = Instantiate(wallAsset, new Vector3(wallX, 0, minZ), Quaternion.identity, transform);
                    wall.name = "Wall Bottom";
                    walls.Add(wall);

                    wall = Instantiate(wallAsset, new Vector3(wallX, 0, maxZ), Quaternion.identity, transform);
                    wall.name = "Wall Top";
                    walls.Add(wall);
                }
            }
            else
            {
                // Place left and right walls
                for (int i = 0; i < height; i++)
                {
                    float wallX = minX;
                    float wallX2 = maxX;
                    float z = minZ + i;

                    GameObject wall = Instantiate(wallAsset, new Vector3(minX, 0, z), Quaternion.identity, transform);
                    wall.name = "Wall Left";
                    walls.Add(wall);

                    wall = Instantiate(wallAsset, new Vector3(maxX, 0, z), Quaternion.identity, transform);
                    wall.name = "Wall Right";
                    walls.Add(wall);
                }
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


        internal void Modify(Bounds bounds)
        {
            foreach (var wall in walls)
            {
                Vector3 wallPos = wall.transform.position;
                if ((wallPos.x > bounds.min.x && wallPos.x < bounds.max.x) && (wallPos.z > bounds.min.y && wallPos.z < bounds.max.y))
                {
                    wall.SetActive(false);
                    Debug.Log("wall pos = " + wall.transform.position);
                }
            }
        }
    }
}