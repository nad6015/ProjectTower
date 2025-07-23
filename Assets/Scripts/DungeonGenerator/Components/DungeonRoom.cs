using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DungeonGenerator
{
    public class DungeonRoom : MonoBehaviour
    {
        public Rect rectBounds { get; set; }
        public Bounds Bounds { get; private set; }

        public Dictionary<GameObject, int> Contents { get; internal set; }
        private List<GameObject> walls;

        private void Awake()
        {
            Contents = new();
        }

        public void Construct(GameObject floorAsset, GameObject wallAsset, DungeonCorridor corridor)
        {
            transform.position = Bounds.min;
            // Place the floor
            GameObject floor = Instantiate(floorAsset, transform);
            floor.transform.localScale = PointUtils.Vec2ToVec3(Bounds.size, 0.5f);

            float width = Bounds.size.x;
            float height = Bounds.size.z;

            float minX = Bounds.min.x;
            float minZ = Bounds.min.z;
            
            float maxX = Bounds.max.x;
            float maxZ = Bounds.max.z;

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
            for (int i = 0; i < width + 1; i++)
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

        public void Modify(DungeonCorridor corridor)
        {
            foreach (Tuple<Rect, DungeonAxis> path in corridor.Paths)
            {
                Rect corridorBounds = path.Item1;
                DungeonAxis axis = path.Item2;

                foreach (GameObject wall in walls)
                {
                    Vector2 position = new(wall.transform.position.x, wall.transform.position.z);

                    if ((position.x > corridorBounds.x && position.x < corridorBounds.xMax) &&
                            (Mathf.Approximately(position.y, corridorBounds.y) || Mathf.Approximately(position.y, corridorBounds.yMax)) ||
                            (position.y > corridorBounds.y && position.y < corridorBounds.yMax) &&
                            (Mathf.Approximately(position.x, corridorBounds.x) || Mathf.Approximately(position.x, corridorBounds.xMax)))
                    {
                        wall.SetActive(false);
                    }
                }

                corridor.Modify(rectBounds);
            }
        }

        // TODO: This seems pointless. Why did I add it?
        private bool WithinBounds(float x, float y)
        {
            Vector2 point = new(x, y);

            return false;
        }

        internal static DungeonRoom Create(Rect bounds, String name)
        {
            // Create gameobject code referenced from  - https://discussions.unity.com/t/how-do-you-create-an-empty-gameobject-in-code-and-add-it-to-the-scene/86380/4
            GameObject gameObj = new(name);
            DungeonRoom dungeonRoom = gameObj.AddComponent<DungeonRoom>();
            dungeonRoom.rectBounds = bounds;
            dungeonRoom.walls = new List<GameObject>();

            return dungeonRoom;
        }

        public static DungeonRoom Create(Bounds bounds, int i)
        {
            // Create gameobject code referenced from  - https://discussions.unity.com/t/how-do-you-create-an-empty-gameobject-in-code-and-add-it-to-the-scene/86380/4
            GameObject gameObj = new("Room " + i);
            DungeonRoom dungeonRoom = gameObj.AddComponent<DungeonRoom>();
            dungeonRoom.Bounds = bounds;
            dungeonRoom.walls = new List<GameObject>();

            return dungeonRoom;
        }
    }
}