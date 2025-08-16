using Assets.DungeonGenerator.Components.Tiles;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DungeonGenerator.Components
{
    public class DungeonRoom : MonoBehaviour
    {
        public Bounds Bounds { get { return DungeonNode.Bounds; } }
        public Dictionary<GameObject, int> Contents { get; internal set; }
        public DungeonNode DungeonNode { get; private set; }
        public RoomType Type { get { return DungeonNode.Type; } }
        private List<GameObject> walls;

        private void Awake()
        {
            Contents = new();
        }

        public void Construct(Tilemap3D tilemap)
        {
            transform.position = Bounds.min;

            float width = Bounds.size.x;
            float height = Bounds.size.z;

            float minX = Bounds.min.x;
            float minZ = Bounds.min.z;

            float maxX = Bounds.max.x;
            float maxZ = Bounds.max.z;

            // Place the floor
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    tilemap.DrawFloor(minX + i, minZ + j, transform);
                }
            }

            // Place the top and bottom walls
            for (int i = 0; i < width; i++)
            {
                float wallX = minX + i;

                // Bottom wall
                walls.Add(tilemap.DrawHorizontalWall(wallX, minZ, transform));

                // Top wall
                walls.Add(tilemap.DrawHorizontalWall(wallX, maxZ, transform));
            }

            // Place left and right walls
            for (int i = 0; i < height; i++)
            {
                float z = minZ + i;

                // Left wall
                walls.Add(tilemap.DrawVerticalWall(minX, z, transform));

                // Right wall
                walls.Add(tilemap.DrawVerticalWall(maxX, z, transform));
            }

            tilemap.DrawRoomCorner(minX, minZ, transform);
            tilemap.DrawRoomCorner(minX, maxZ, transform);
            tilemap.DrawRoomCorner(maxX, minZ, transform);
            tilemap.DrawRoomCorner(maxX, maxZ, transform);
        }

        public void Modify(Bounds cBounds, Tilemap3D tilemap)
        {
            if (!cBounds.Intersects(Bounds))
            {
                return;
            }

            Vector3 firstPos = Vector3.zero;

            foreach (GameObject wall in walls)
            {
                Vector3 position = wall.transform.position;

                if (position.x > cBounds.min.x && position.x < cBounds.max.x &&
                    (Mathf.Approximately(position.z, cBounds.min.z) || Mathf.Approximately(position.z, cBounds.max.z)))
                {
                    wall.SetActive(false);
                    if (firstPos == Vector3.zero)
                    {
                        firstPos = position;
                    }
                }

                if (position.z > cBounds.min.z && position.z < cBounds.max.z &&
                    (Mathf.Approximately(position.x, cBounds.min.x) || Mathf.Approximately(position.x, cBounds.max.x)))
                {
                    wall.SetActive(false);
                    if (firstPos == Vector3.zero)
                    {
                        firstPos = position;
                    }
                }
            }
            bool isHorizontal = Mathf.Approximately(firstPos.x, cBounds.min.x) || Mathf.Approximately(firstPos.x, cBounds.max.x);
            tilemap.DrawCorridorArch(
                isHorizontal ? cBounds.max.x : cBounds.min.x,
                isHorizontal ? cBounds.min.z : cBounds.max.z,
                isHorizontal, transform);

            tilemap.DrawCorridorArch(cBounds.min.x, cBounds.min.z, isHorizontal, transform);
        }

        public static DungeonRoom Create(DungeonNode node)
        {
            // Create gameobject code referenced from  - https://discussions.unity.com/t/how-do-you-create-an-empty-gameobject-in-code-and-add-it-to-the-scene/86380/4
            GameObject gameObj = new("Room " + node.Id);
            DungeonRoom dungeonRoom = gameObj.AddComponent<DungeonRoom>();
            dungeonRoom.DungeonNode = node;
            dungeonRoom.walls = new List<GameObject>();

            return dungeonRoom;
        }

        internal void Populate(DungeonRepresentation dungeon)
        {
            float spawnRate = dungeon.Parameter<int>(DungeonParameter.EnemySpawnRate) / 100f;
            Range<int> enemiesPerRoom = dungeon.Parameter<Range<int>>(DungeonParameter.EnemiesPerRoom);

            if (Random.value > spawnRate || dungeon.Components.enemies.Count == 0)
            {
                return;
            }
            int count = Random.Range(enemiesPerRoom.min, enemiesPerRoom.max);

            Contents.Add(dungeon.Components.enemies[0], count);
        }
    }
}