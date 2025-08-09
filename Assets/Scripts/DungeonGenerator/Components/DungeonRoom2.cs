using Assets.DungeonGenerator;
using Assets.DungeonGenerator.Components;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.DungeonGenerator.Components
{
    public class DungeonRoom : MonoBehaviour
    {
        public Bounds Bounds { get; private set; }
        public Dictionary<GameObject, int> Contents { get; internal set; }


        private List<GameObject> walls;

        private void Awake()
        {
            Contents = new();
        }

        public void Construct(DungeonComponents components)
        {
            GameObject floorAsset = components.floorTile;
            GameObject wallAsset = components.wallTile;

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

            // Place the top and bottom walls
            for (int i = 0; i < width + 1; i++)
            {
                float wallX = minX + i;

                // Bottom wall
                GameObject wall = Instantiate(wallAsset, new Vector3(wallX, 0, minZ), Quaternion.identity, transform);
                wall.name = "Wall Bottom";
                walls.Add(wall);

                // Top wall
                wall = Instantiate(wallAsset, new Vector3(wallX, 0, maxZ), Quaternion.identity, transform);
                wall.name = "Wall Top";
                walls.Add(wall);
            }

            // Place left and right walls
            for (int i = 0; i < height + 1; i++)
            {
                float z = minZ + i;

                GameObject wall = Instantiate(wallAsset, new Vector3(minX, 0, z), Quaternion.identity, transform);
                wall.name = "Wall Left";
                wall.transform.GetChild(0).Rotate(Vector3.up, 90);
                walls.Add(wall);


                wall = Instantiate(wallAsset, new Vector3(maxX, 0, z), Quaternion.identity, transform);
                wall.name = "Wall Right";
                wall.transform.GetChild(0).Rotate(Vector3.up, 90);
                walls.Add(wall);
            }
        }

        public void Modify(DungeonCorridor corridor)
        {
            Bounds cBounds = corridor.Bounds;

            foreach (GameObject wall in walls)
            {
                Vector3 position = wall.transform.position;

                if (position.x > cBounds.min.x && position.x < cBounds.max.x &&
                    (Mathf.Approximately(position.z, cBounds.min.z) || Mathf.Approximately(position.z, cBounds.max.z)))
                {
                    wall.SetActive(false);
                }

                if (position.z > cBounds.min.z && position.z < cBounds.max.z &&
                    (Mathf.Approximately(position.x, cBounds.min.x) || Mathf.Approximately(position.x, cBounds.max.x)))
                {
                    wall.SetActive(false);
                }
            }
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

        internal void Populate(Dungeon dungeon)
        {
            float spawnRate = dungeon.Parameter("enemySpawnRate").Value();
            Range<float> enemiesPerRoom = dungeon.Parameter("enemiesPerRoom").Range();

            if (UnityEngine.Random.value > spawnRate)
            {
                return;
            }
            int count = Mathf.RoundToInt(UnityEngine.Random.Range(enemiesPerRoom.min, enemiesPerRoom.max));

            Contents.Add(dungeon.Components.enemies[0], count);
        }
    }

    public class DungeonFlowNode
    {
     
        public RoomType Type { get; }

        public DungeonFlowNode(string type)
        {
            switch (type.ToLower())
            {
                case "explore":
                {
                    Type = RoomType.EXPLORE;
                    break;
                }
                case "combat":
                {
                    Type = RoomType.COMBAT;
                    break;
                }
                case "item":
                {
                    Type = RoomType.ITEM;
                    break;
                }
                case "start":
                {
                    Type = RoomType.START;
                    break;
                }
                case "end":
                {
                    Type = RoomType.END;
                    break;
                }
            }
        }

        public override bool Equals(object obj)
        {
            return obj is DungeonFlowNode room &&
                   Type == room.Type;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type);
        }

        public override string ToString()
        {
            return Type.ToString();
        }
    }
}