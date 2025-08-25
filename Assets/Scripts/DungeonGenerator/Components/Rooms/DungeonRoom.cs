using Assets.DungeonGenerator.Components.Tiles;
using Assets.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using static Assets.Utilities.GameObjectUtilities;

namespace Assets.DungeonGenerator.Components
{
    public class DungeonRoom : MonoBehaviour
    {
        public Bounds Bounds { get { return DungeonNode.Bounds; } }
        public List<Tuple<GameObject, Vector3>> Contents { get; private set; }
        public DungeonNode DungeonNode { get; private set; }
        public RoomType Type { get { return DungeonNode.Type; } }

        private List<GameObject> _walls;
        private Bounds _safeArea; // Where in the room is safe to place items

        private void Awake()
        {
            Contents = new();
            _walls = new();
        }

        public void Construct(DungeonTilemap tilemap)
        {
            BoundsInt bounds = DungeonComponentUtils.BoundsToBoundsInt(Bounds); // Convert to int for accurate tile placement
            transform.position = bounds.min;

            // Create an empty game object to contain the floor tiles
            GameObject floor = new("Floor");
            floor.transform.SetParent(transform);

            // Place the floors
            DungeonComponentUtils.DrawFloor(tilemap, bounds, floor.transform);

            // Create an empty game object to contain the wall tiles
            GameObject walls = new("Walls");
            walls.transform.SetParent(transform);
            // Place top and bottom walls
            _walls.AddRange(DungeonComponentUtils.DrawTopAndBottomWalls(tilemap, bounds, walls.transform));

            // Place left and right walls
            _walls.AddRange(DungeonComponentUtils.DrawLeftAndRightWalls(tilemap, bounds, walls.transform));

            // Place room corner decorations
            tilemap.DrawRoomCorner(bounds.min, transform);
            tilemap.DrawRoomCorner(bounds.xMin, bounds.zMax, transform);
            tilemap.DrawRoomCorner(bounds.xMax, bounds.zMin, transform);
            tilemap.DrawRoomCorner(bounds.max, transform);
        }

        public void Modify(Bounds cBounds)
        {
            if (!cBounds.Intersects(Bounds))
            {
                return;
            }

            foreach (GameObject wall in _walls)
            {
                if (cBounds.Contains(wall.transform.position))
                {
                    wall.SetActive(false);
                }
            }
        }

        public virtual void InstaniateContents(DungeonRepresentation dungeon) { }

        public static DungeonRoom Create(DungeonNode node)
        {
            string name = "Room " + node.Id + " - " + node.Type;
            DungeonRoom dungeonRoom;
            switch (node.Type)
            {
                case RoomType.Start:
                {
                    dungeonRoom = NewGameObjectWithComponent<DungeonStartRoom>(name);
                    break;
                }
                case RoomType.End:
                {
                    dungeonRoom = NewGameObjectWithComponent<DungeonEndRoom>(name);
                    break;
                }
                case RoomType.Combat:
                {
                    dungeonRoom = NewGameObjectWithComponent<CombatRoom>(name);
                    break;
                }
                case RoomType.Lock:
                {
                    dungeonRoom = NewGameObjectWithComponent<LockedRoom>(name);
                    break;
                }
                default:
                {
                    dungeonRoom = NewGameObjectWithComponent<DungeonRoom>(name);
                    break;
                }
            }

            // Setup safe area for later item placement
            dungeonRoom._safeArea = new Bounds(node.Bounds.center, node.Bounds.size);
            dungeonRoom._safeArea.Expand(-DungeonTilemap.TileUnit * 2);
            dungeonRoom.DungeonNode = node;
            return dungeonRoom;
        }

        internal virtual void Populate(DungeonRepresentation dungeon)
        {
            SpawnEnemies(dungeon);
            PlaceProps(dungeon);
        }

        protected virtual void SpawnEnemies(DungeonRepresentation dungeon)
        {
            float spawnRate = dungeon.Parameter<int>(DungeonParameter.EnemySpawnRate) / 100f;
            Range<int> enemiesPerRoom = dungeon.Parameter<Range<int>>(DungeonParameter.EnemiesPerRoom);
            List<GameObject> enemies = dungeon.Components.enemies;

            if (Random.value < spawnRate && enemies.Count > 0)
            {
                int count = Random.Range(enemiesPerRoom.min, enemiesPerRoom.max);
                GameObject enemy = dungeon.Components.enemies[0];
                Contents.Add(new(enemy, enemy.transform.position + PointUtils.RandomPointWithinBounds(Bounds)));
            }
        }

        protected virtual void PlaceProps(DungeonRepresentation dungeon)
        {
            int itemCount = dungeon.RandomItemCount();
            
            DungeonTilemap tilemap = dungeon.Components.tilemap;
            for (int i = 0; i < itemCount; i++)
            {
                DungeonProp prop = tilemap.GetProp();
                if (prop == null) // if null, then shufflebag is empty
                {
                    break;
                }
                Vector3 pos = prop.transform.position;

                Contents.Add(new(prop.gameObject, pos + PointUtils.RandomPointWithinBounds(_safeArea)));
            }
        }
    }
}