using UnityEngine;
using Assets.DungeonGenerator.Components;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;
using Assets.DungeonGenerator.Components.Tiles;
using Unity.AI.Navigation;

namespace Assets.DungeonGenerator
{
    /// <summary>
    /// Generates a dungeon using a random walk algorithm.
    /// </summary>
    public class RandomWalk : IDungeonAlgorithm
    {
        private Dictionary<Bounds, DungeonRoom> _roomBounds;
        private Dictionary<Bounds, DungeonCorridor> _corridors;

        private DungeonRepresentation _dungeon;
        private DungeonComponents _components;
        private readonly Transform _dungeonTransform;
        private readonly List<Vector3> _shuffleBag;

        public RandomWalk(Transform transform)
        {
            _dungeonTransform = transform;
            _shuffleBag = new List<Vector3>()
            {
                Vector3.back,
                Vector3.forward,
                Vector3.left,
                Vector3.right
            };
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void GenerateDungeon(DungeonRepresentation dungeon)
        {
            _dungeon = dungeon;
            _components = dungeon.Components;
            _corridors = new();
            _roomBounds = new();

            CreateDungeonRooms();
            ConstructDungeon();
            GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<NavMeshSurface>().BuildNavMesh();
            PlaceContent();
        }

        /// <summary>
        /// Creates the data structures for the rooms and corridors of the dungeon. The algorithm used works as follows:
        /// 
        ///    1. Choose a random direction.
        ///    2. Center a room on (0,0,0) and use the random direction to decide with axis to place it on.
        ///    3. Using the random direction place a room along the axis.
        ///    4. Connect the two rooms with a corridor.
        ///    5. Choose a new random direction.
        ///    6. Repeat steps 3 to 5 using the last created room as the connection point to the new one. Stop when the desire number of rooms is reached.
        /// </summary>
        private void CreateDungeonRooms()
        {
            int negativeDirOffset = Mathf.FloorToInt(_dungeon.Parameter<Range<Vector3>>(DungeonParameter.RoomSize).max.magnitude);
            Vector3 dir = RandomDirection();

            Bounds lastRoom = RandomRoom(Vector3.zero, Vector3.zero, dir, dir.x != 0);
            _roomBounds.Add(lastRoom, null);

            DungeonLayout layout = _dungeon.Layout;
            DungeonNode node = layout.FirstNode;
            node.Bounds = lastRoom;
            HashSet<DungeonNode> builtNodes = new() { node };

            LinkNodes(layout.FirstNode, dir, negativeDirOffset, builtNodes);
        }

        void LinkNodes(DungeonNode node, Vector3 dir, int negativeDirOffset, HashSet<DungeonNode> nodes)
        {
            var lastRoom = node.Bounds;
            foreach (var n in node.LinkedNodes)
            {
                if (nodes.Contains(n))
                {
                    continue;
                }
                for (var i = 0; i < 4; i++)
                {
                    Bounds nextRoom;
                    Bounds corridor;

                    if (dir == Vector3.right)
                    {
                        nextRoom = RandomRoom(new(lastRoom.max.x, 0, lastRoom.min.z), lastRoom.max, dir, true);
                        corridor = CreateCorridor(lastRoom, nextRoom, true);
                    }
                    else if (dir == Vector3.forward)
                    {
                        nextRoom = RandomRoom(new(lastRoom.min.x, 0, lastRoom.max.z), lastRoom.max, dir, false);
                        corridor = CreateCorridor(lastRoom, nextRoom, false);
                    }
                    else if (dir == Vector3.left)
                    {
                        nextRoom = RandomRoom(new(lastRoom.min.x - 0, 0, lastRoom.min.z), lastRoom.max, dir, true);
                        corridor = CreateCorridor(nextRoom, lastRoom, true);
                    }
                    else // dir == Vector3.down)
                    {
                        nextRoom = RandomRoom(new(lastRoom.min.x, 0, lastRoom.min.z - 0), lastRoom.max, dir, false);
                        corridor = CreateCorridor(nextRoom, lastRoom, false);
                    }
                    dir = RandomDirection();
                    // If a room or a corridor already exists in that area, then loop again.
                    if (CanPlaceRoom(nextRoom, corridor))
                    {
                        _corridors.Add(corridor, null);
                        _roomBounds.Add(nextRoom, null);
                        n.Bounds = nextRoom;
                        nodes.Add(n);

                        LinkNodes(n, dir, negativeDirOffset, nodes);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Constructs the rooms and corridors of the dungeon. It also ensures that no overlapping corridor tiles block entry to rooms.
        /// </summary>
        private void ConstructDungeon()
        {
            // Dictionary upsert code referenced from - https://stackoverflow.com/questions/1243717/how-to-update-the-value-stored-in-dictionary-in-c
            for (int i = 0; i < _corridors.Count; i++)
            {
                var corridor = _corridors.ElementAt(i);
                DungeonCorridor dungeonCorridor = DungeonCorridor.Create(corridor.Key, i);
                dungeonCorridor.Construct(_components.tilemap, _dungeon.Parameter<Vector3>(DungeonParameter.CorridorSize));
                dungeonCorridor.transform.SetParent(_dungeonTransform);
                _corridors[corridor.Key] = dungeonCorridor;
            }

            foreach (var node in _dungeon.Layout)
            {
                var roomBounds = node.Bounds;
                DungeonRoom room = DungeonRoom.Create(node);
                room.Construct(_components.tilemap);
                room.transform.SetParent(_dungeonTransform);
                _roomBounds[roomBounds] = room;
                _dungeon.AddDungeonRoom(room);
            }

            foreach (var room in _roomBounds.Values)
            {
                room.Populate(_dungeon);
                foreach (var corridor in _corridors.Values)
                {
                    room.Modify(corridor.Bounds);
                }
            }
        }

        /// <summary>
        /// Places the content in each room, including the dungeon exit and player starting point.
        /// </summary>
        private void PlaceContent()
        {
            foreach (var room in _roomBounds.Values)
            {
                room.InstaniateContents(_dungeon);
                foreach (var content in room.Contents)
                {
                    GameObject.Instantiate(content.Item1, content.Item2, Quaternion.identity, room.transform);
                }
            }
        }

        /// <summary>
        /// Creates a corridor between two rooms.
        /// </summary>
        /// <param name="b1">the bounds of the first room</param>
        /// <param name="b2">the bounds of the second room</param>
        /// <param name="isHorizontal">is the corridor a horizontal connection or not</param>
        /// 
        /// <returns>the bounds of the corridor</returns>
        private Bounds CreateCorridor(Bounds b1, Bounds b2, bool isHorizontal)
        {
            Bounds corridorBounds = new();
            Vector3 corridorSize = _dungeon.Parameter<Vector3>(DungeonParameter.CorridorSize);

            if (isHorizontal)
            {
                int minX = Mathf.FloorToInt(b1.max.x);
                int maxX = Mathf.FloorToInt(b2.min.x + DungeonTilemap.TileUnit);

                float minZ = Mathf.Max(b1.min.z, b2.min.z);
                float maxZ = Mathf.Min(b1.max.z, b2.max.z) - corridorSize.z;
                float z = Random.Range(minZ, maxZ);

                corridorBounds.SetMinMax(new(minX, 0, z), new(maxX, 0, z + corridorSize.z));
            }
            else
            {
                int minZ = Mathf.FloorToInt(b1.max.z);
                int maxZ = Mathf.FloorToInt(b2.min.z + DungeonTilemap.TileUnit);

                float minX = Mathf.Max(b1.min.x, b2.min.x);
                float maxX = Mathf.Min(b1.max.x, b2.max.x) - corridorSize.x;
                float x = Random.Range(minX, maxX);

                corridorBounds.SetMinMax(new(x, 0, minZ), new(x + corridorSize.x, 0, maxZ));
            }
            return corridorBounds;
        }

        /// <summary>
        /// Creates a randomly sized and placed room.
        /// </summary>
        /// <param name="min">the minimum size of the room</param>
        /// <param name="max">the maximum size of the room</param>
        /// <param name="isHorizontal">should the room be placed along the x-axis</param>
        /// <returns>the bounds of the room</returns>
        private Bounds RandomRoom(Vector3 min, Vector3 max, Vector3 dir, bool isHorizontal)
        {
            Range<Vector3> roomSizeParam = _dungeon.Parameter<Range<Vector3>>(DungeonParameter.RoomSize);
            Vector3 corridorSize = _dungeon.Parameter<Vector3>(DungeonParameter.CorridorSize);
            int roomOffset = (int)corridorSize.x; //Random.Range(Mathf.RoundToInt(corridorSize.x) + DungeonTilemap.TileUnit, 5); // Distance between rooms

            Vector3 roomSize = PointUtils.RandomSize(roomSizeParam.min, roomSizeParam.max);
            Vector3 roomCenter = PointUtils.RandomPointWithinRange(min, max);

            if (isHorizontal)
            {
                roomCenter.x += dir.x > 0 ? roomOffset + roomSize.x : -(roomOffset + roomSize.x);
            }
            else
            {
                roomCenter.z += dir.z > 0 ? roomOffset + roomSize.z : -(roomOffset + roomSize.z);
            }

            return new Bounds(roomCenter, roomSize);
        }

        /// <summary>
        /// Chooses a random direction from the following list:
        ///     - Right (1,0,0)
        ///     - Left (-1,0,0)
        ///     - Up (0,0,1)
        ///     - Down (0,0,-1)
        /// Uses the dungeon parameters to decide the bias between directions.
        /// </summary>
        /// <returns>a randomly chosen direction</returns>
        private Vector3 RandomDirection()
        {
            if (_shuffleBag.Count == 0)
            {
                _shuffleBag.Add(Vector3.back);
                _shuffleBag.Add(Vector3.forward);
                _shuffleBag.Add(Vector3.left);
                _shuffleBag.Add(Vector3.right);
            }

            int randomIndex = Mathf.RoundToInt(Random.value * (_shuffleBag.Count - 1));

            Vector3 dir = _shuffleBag[randomIndex];
            _shuffleBag.RemoveAt(randomIndex);

            return dir;
        }

        /// <summary>
        /// Checks if a room can be placed in the given bounds.
        /// </summary>
        /// <returns>true if no other room is in the location.</returns>
        private bool CanPlaceRoom(Bounds newRoom, Bounds newCorridor)
        {
            foreach (var room in _roomBounds.Keys)
            {
                if (room.Intersects(newRoom))
                {
                    return false;
                }
            }
            foreach (var corridor in _corridors.Keys)
            {
                if (corridor.Intersects(newCorridor) || corridor.Intersects(newRoom))
                {
                    return false;
                }
            }
            return true;
        }
    }
}