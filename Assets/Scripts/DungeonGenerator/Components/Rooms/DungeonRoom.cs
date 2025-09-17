using Assets.DungeonGenerator.Components.Tiles;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using static Assets.Utilities.GameObjectUtilities;
using Assets.DungeonGenerator.Components.Rooms;

namespace Assets.DungeonGenerator.Components
{
    public class DungeonRoom : MonoBehaviour
    {
        public Bounds Bounds { get { return DungeonNode.Bounds; } }
        public List<Tuple<GameObject, Vector3>> Contents { get; private set; }
        public DungeonNode DungeonNode { get; private set; }
        public RoomType Type { get { return DungeonNode.Type; } }

        protected List<GameObject> _walls;
        protected GameObject _floor;
        private Bounds _safeArea; // Where in the room is safe to place items

        private void Awake()
        {
            Contents = new();
            _walls = new();

            // Create an empty game object to contain the floor tiles
            _floor = new("Floor");
            _floor.transform.SetParent(transform);
        }

        public void Construct(DungeonTilemap tilemap)
        {
            BoundsInt bounds = DungeonComponentUtils.BoundsToBoundsInt(Bounds); // Convert to int for accurate tile placement
            transform.position = bounds.min;

            // Place the floors
            DungeonComponentUtils.DrawFloor(tilemap, bounds, _floor.transform);

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
                case RoomType.RestPoint:
                {
                    dungeonRoom = NewGameObjectWithComponent<RestPointRoom>(name);
                    break;
                }
                case RoomType.Boss:
                {
                    dungeonRoom = NewGameObjectWithComponent<BossRoom>(name);
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
            SpawnEnemies(dungeon, dungeon.Parameter<int>(DungeonParameter.EnemySpawnRate));
            PlaceProps(dungeon);
        }

        protected virtual void SpawnEnemies(DungeonRepresentation dungeon, int enemySpawnRate)
        {
            float spawnRate = enemySpawnRate / 100f;
            Range<int> enemiesPerRoom = dungeon.Parameter<Range<int>>(DungeonParameter.EnemiesPerRoom);
            List<GameObject> enemies = dungeon.Components.enemies;

            if (Random.value < spawnRate && enemies.Count > 0)
            {
                int count = Random.Range(enemiesPerRoom.min, enemiesPerRoom.max);
                GameObject enemy = dungeon.Components.enemies[0];
                Contents.Add(new(enemy, enemy.transform.position + PointUtils.RandomPointWithinBounds(_safeArea)));
            }
        }

        protected virtual void PlaceProps(DungeonRepresentation dungeon)
        {
            int itemCount = dungeon.RandomItemCount();

            DungeonTilemap tilemap = dungeon.Components.tilemap;
            for (int i = 0; i < itemCount; i++)
            {
                DungeonTile prop = tilemap.GetProp();
                if (prop == null) // if null, then shufflebag is empty
                {
                    break;
                }
                Vector3 pos = prop.transform.position;

                Contents.Add(new(prop.gameObject, pos + PointUtils.RandomPointWithinBounds(_safeArea)));
            }
        }

        protected DungeonDoor LockClosestDoor()
        {
            DungeonNode lockedRoom = null;

            if (DungeonNode.LinkedNodes.Count > 2)
            {
                foreach (var node in DungeonNode.LinkedNodes)
                {
                    HashSet<DungeonNode> visitedNodes = new()
                    {
                        node, DungeonNode
                    };
                    DungeonNode fnode = FindPathTo(RoomType.End, node.LinkedNodes, visitedNodes);
                    if (fnode != null)
                    {
                        lockedRoom = node;
                        break;
                    }
                }
            }
            else
            {
                lockedRoom = DungeonNode.LinkedNodes[0];
            }

            DungeonCorridor corridor = null;
            foreach (var c in FindObjectsByType<DungeonCorridor>(FindObjectsSortMode.None))
            {
                if (c.Bounds.Intersects(Bounds) && c.Bounds.Intersects(lockedRoom.Bounds))
                {
                    corridor = c;
                    break;
                }
            }

            DungeonDoor door = corridor.Doors.Item1;

            return door;
        }

        private DungeonNode FindPathTo(RoomType type, List<DungeonNode> connectedNodes, HashSet<DungeonNode> visitedNodes)
        {
            foreach (DungeonNode node in connectedNodes)
            {
                if (visitedNodes.Contains(node))
                {
                    continue;
                }
                visitedNodes.Add(node);
                if (node.Type == type)
                {
                    return node;
                }
                else
                {
                    return FindPathTo(type, node.LinkedNodes, visitedNodes);
                }
            }
            return null;
        }
    }
}