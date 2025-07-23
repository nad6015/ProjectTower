using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DungeonGenerator
{
    public class DungeonCorridor : MonoBehaviour
    {
        static int corridorId = 0;
        public List<Tuple<Rect, DungeonAxis>> Paths { get; private set; }
        public Bounds Bounds { get; private set; }

        private List<GameObject> walls = new();


        // Place corridor
        internal void Construct(DungeonComponents components)
        {
            // TODO: Construct should be idempotent or not called more than once
            if (walls.Count > 0)
            {
                return;
            }
            GameObject corridorAsset = components.corridorTile;
            GameObject floorAsset = components.floorTile;

            ConstructCorridor(corridorAsset, floorAsset, DungeonAxis.HORIZONTAL);
        }

        private void ConstructCorridor(GameObject wallAsset, GameObject floorAsset, DungeonAxis axis)
        {
            float width = Bounds.size.x;
            float height = Bounds.size.z;

            float minX = Bounds.min.x;
            float minZ = Bounds.min.z;

            float maxX = Bounds.max.x;
            float maxZ = Bounds.max.z;

            transform.position = Bounds.min;
            
            // Don't place anything if corridor has no height or width. This means the rooms share a boundary
            if ((Mathf.Approximately(width, 0) || Mathf.Approximately(height, 0)))
            {
                return;
            }

            GameObject floor = Instantiate(floorAsset, transform);
            floor.transform.localScale = PointUtils.Vec2ToVec3(Bounds.size, 0.5f);

            // Place the top wall
            for (int i = 0; i < width; i++)
            {
                float wallX = minX + i;
                float wallZ = minZ;

                GameObject wall = Instantiate(wallAsset);
                wall.transform.localPosition = new Vector3(wallX, 0, wallZ);
                wall.name = "Wall cap";
                wall.transform.SetParent(transform);
                walls.Add(wall);
            }

            // Place left and right walls
            for (int i = 0; i < height; i++)
            {
                float wallX = minX;
                float wallX2 = maxX;
                float wallY = minZ + i;

                if (!WithinBounds(wallX, wallY))
                {
                    GameObject wall = Instantiate(wallAsset);
                    wall.name = "Wall length";
                    wall.transform.localPosition = new Vector3(wallX, 0, wallY);
                    wall.transform.SetParent(transform);
                    walls.Add(wall);
                }

                if (!WithinBounds(wallX2, wallY))
                {
                    GameObject wall2 = Instantiate(wallAsset);
                    wall2.name = "Wall length";
                    wall2.transform.localPosition = new Vector3(wallX2, 0, wallY);
                    wall2.transform.SetParent(transform);
                    walls.Add(wall2);
                }
            }

            // Place top wall
            for (int i = 0; i < width; i++)
            {
                float wallX = minX + i;
                float wallY = maxZ;

                if (!WithinBounds(wallX, wallY))
                {
                    GameObject wall = Instantiate(wallAsset);
                    wall.transform.localPosition = new Vector3(wallX, 0, wallY);
                    wall.name = "Wall cap";
                    wall.transform.SetParent(transform);
                    walls.Add(wall);
                }
            }
        }

        private bool WithinBounds(float wallX, float wallY)
        {
            return false;
        }

        internal static DungeonCorridor Create(List<Tuple<Rect, DungeonAxis>> paths)
        {
            // Create gameobject code referenced from  - https://discussions.unity.com/t/how-do-you-create-an-empty-gameobject-in-code-and-add-it-to-the-scene/86380/4
            GameObject gameObj = new("Corridor " + corridorId++);
            DungeonCorridor corridor = gameObj.AddComponent<DungeonCorridor>();
            corridor.Paths = paths;
            return corridor;
        }

        public static DungeonCorridor Create(Bounds bounds, int id)
        {
            // Create gameobject code referenced from  - https://discussions.unity.com/t/how-do-you-create-an-empty-gameobject-in-code-and-add-it-to-the-scene/86380/4
            GameObject gameObj = new("Corridor " + id);
            DungeonCorridor corridor = gameObj.AddComponent<DungeonCorridor>();
            corridor.Bounds = bounds;
            return corridor;
        }


        internal void Modify(Rect bounds)
        {
            Debug.Log("Bounds = " + bounds);
            foreach (var wall in walls)
            {
                Vector3 wallPos = wall.transform.position;
                if ((wallPos.x > bounds.x && wallPos.x < bounds.xMax) && (wallPos.z > bounds.y && wallPos.z < bounds.yMax))
                {
                    wall.SetActive(false);
                    Debug.Log("wall pos = " + wall.transform.position);
                }
            }
        }
    }
}