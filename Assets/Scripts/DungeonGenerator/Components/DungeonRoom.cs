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
            BoundsInt bounds = DungeonComponentUtils.BoundsToBoundsInt(Bounds); // Convert to int for accurate tile placement
            transform.position = bounds.min;

            // Place the floor
            DungeonComponentUtils.DrawFloor(tilemap, bounds, transform);

            // Place top and bottom walls
            walls.AddRange(DungeonComponentUtils.DrawTopAndBottomWalls(tilemap, bounds, transform));

            // Place left and right walls
            walls.AddRange(DungeonComponentUtils.DrawLeftAndRightWalls(tilemap, bounds, transform));

            // Place room corner decorations
            tilemap.DrawRoomCorner(bounds.min, transform);
            tilemap.DrawRoomCorner(bounds.xMin, bounds.zMax, transform);
            tilemap.DrawRoomCorner(bounds.xMax, bounds.zMin, transform);
            tilemap.DrawRoomCorner(bounds.max, transform);
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
                float x = position.x + Tilemap3D.TileUnit;
                float z = position.z + Tilemap3D.TileUnit;

                if (cBounds.Contains(position))
                {
                    wall.SetActive(false);
                    if (firstPos == Vector3.zero)
                    {
                        firstPos = position;
                    }
                }

                if (z > cBounds.min.z && z < cBounds.max.z &&
                    (Mathf.Approximately(x, cBounds.min.x) || Mathf.Approximately(x, cBounds.max.x)))
                {
                    wall.SetActive(false);
                    if (firstPos == Vector3.zero)
                    {
                        firstPos = position;
                    }
                }
            }
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