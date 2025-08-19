using Assets.DungeonGenerator.Components.Tiles;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.DungeonGenerator.Components
{
    public class DungeonRoom : MonoBehaviour
    {
        public Bounds Bounds { get { return DungeonNode.Bounds; } }
        public List<Tuple<GameObject, Vector3>> Contents { get; private set; }
        public DungeonNode DungeonNode { get; private set; }
        public RoomType Type { get { return DungeonNode.Type; } }

        protected float _maxTileUnitsUnavailable = 0.5f;
        private List<GameObject> _walls;

        private void Awake()
        {
            Contents = new();
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

        public static DungeonRoom Create(DungeonNode node)
        {
            // Create gameobject code referenced from  - https://discussions.unity.com/t/how-do-you-create-an-empty-gameobject-in-code-and-add-it-to-the-scene/86380/4
            GameObject gameObj = new("Room " + node.Id);
            DungeonRoom dungeonRoom;
            switch (node.Type)
            {
                case RoomType.Start:
                {
                    dungeonRoom = gameObj.AddComponent<DungeonStartRoom>();
                    break;
                }
                case RoomType.End:
                {
                    dungeonRoom = gameObj.AddComponent<DungeonEndRoom>();
                    break;
                }
                case RoomType.Combat:
                {
                    dungeonRoom = gameObj.AddComponent<CombatRoom>();
                    break;
                }
                case RoomType.Lock:
                {
                    dungeonRoom = gameObj.AddComponent<LockedRoom>();
                    break;
                }
                default:
                {
                    dungeonRoom = gameObj.AddComponent<DungeonRoom>();
                    break;
                }
            }

            dungeonRoom.DungeonNode = node;
            dungeonRoom._walls = new List<GameObject>();

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
            int loopCount = 0; // TODO: Remove later
            int availableTileUnits = Mathf.RoundToInt((Bounds.size.z * Bounds.size.x) * _maxTileUnitsUnavailable);
            DungeonTilemap tilemap = dungeon.Components.tilemap;
            while (availableTileUnits > 0 && loopCount < 5)
            {
                DungeonProp prop = tilemap.GetProp();
                if (prop == null) // if null, then shufflebag is empty
                { 
                    break;
                }
                Vector3 pos = prop.transform.position;
                switch (prop.Type)
                {
                    case AreaType.WALL:
                    {
                        Contents.Add(new(prop.gameObject, pos + new Vector3(Bounds.center.x, 0, Bounds.max.z - DungeonTilemap.TileUnit)));
                        break;
                    }
                    case AreaType.FLOOR:
                    {
                        Contents.Add(new(prop.gameObject, pos + PointUtils.RandomPointWithinBounds(Bounds)));
                        break;
                    }
                }
                availableTileUnits -= Mathf.RoundToInt(prop.TileSize.x * prop.TileSize.y);
                loopCount++;
            }
        }
    }
}