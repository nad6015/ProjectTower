using UnityEngine;
using Assets.DungeonGenerator.Components;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.DungeonGenerator.Components;

namespace Assets.DungeonGenerator
{
    using Random = UnityEngine.Random;

    /// <summary>
    /// Generates a dungeon using a random walk algorithm.
    /// </summary>
    public class RandomWalk : IDungeonAlgorithm
    {
        private Dictionary<Bounds, DungeonRoom> _roomBounds;
        private Dictionary<Bounds, DungeonCorridor> _corridors;

        private Dungeon _dungeon;
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
        /// 
        public void GenerateDungeon(Dungeon dungeon)
        {
            _dungeon = dungeon;
            _components = dungeon.Components;
            _corridors = new();
            _roomBounds = new();

            CreateDungeonRepresentation();
            ConstructDungeon();
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
        private void CreateDungeonRepresentation()
        {
            float negativeDirOffset = _dungeon.Parameter("roomSize").VectorRange().max.magnitude;
            Vector3 dir = RandomDirection();

            Bounds lastRoom = RandomRoom(Vector3.zero, Vector3.zero, dir, dir.x != 0);
            _roomBounds.Add(lastRoom, null);
            Debug.Log(lastRoom);

            while (_roomBounds.Count < _dungeon.Parameter("roomCount").Value())
            {
                Bounds nextRoom;
                Bounds corridor;

                if (dir == Vector3.right)
                {
                    nextRoom = RandomRoom(new(lastRoom.max.x, 0, lastRoom.min.z), lastRoom.max, dir, true);
                    corridor = CreateCorridor(lastRoom, nextRoom, nextRoom.min.x - lastRoom.max.x, true);
                }
                else if (dir == Vector3.forward)
                {
                    nextRoom = RandomRoom(new(lastRoom.min.x, 0, lastRoom.max.z), lastRoom.max, dir, false);
                    corridor = CreateCorridor(lastRoom, nextRoom, nextRoom.min.z - lastRoom.max.z, false);
                }
                else if (dir == Vector3.left)
                {
                    nextRoom = RandomRoom(new(lastRoom.min.x - negativeDirOffset, 0, lastRoom.min.z), lastRoom.max, dir, true);
                    corridor = CreateCorridor(nextRoom, lastRoom, lastRoom.min.x - nextRoom.max.x, true);
                }
                else // dir == Vector3.down)
                {
                    nextRoom = RandomRoom(new(lastRoom.min.x, 0, lastRoom.min.z - negativeDirOffset), lastRoom.max, dir, false);
                    corridor = CreateCorridor(nextRoom, lastRoom, lastRoom.min.z - nextRoom.max.z, false);
                }

                // If a room already exists in that area, then loop again.
                if (CanPlaceRoom(nextRoom, corridor))
                {
                    _corridors.Add(corridor, null);
                    _roomBounds.Add(nextRoom, null);
                    lastRoom = nextRoom;
                }

                dir = RandomDirection();
            }
        }

        /// <summary>
        /// Constructs the rooms and corridors of the dungeon. It also ensures that no overlapping corridor tiles block entry to rooms.
        /// </summary>
        private void ConstructDungeon()
        {
            // Dictionary upsert code referenced from - https://stackoverflow.com/questions/1243717/how-to-update-the-value-stored-in-dictionary-in-c
            for (int i = 0; i < _roomBounds.Count; i++)
            {
                var roomBounds = _roomBounds.ElementAt(i);
                DungeonRoom room = DungeonRoom.Create(roomBounds.Key, i);
                room.Construct(_components);
                room.Populate(_dungeon);
                room.transform.SetParent(_dungeonTransform);
                _roomBounds[roomBounds.Key] = room;
            }

            for (int i = 0; i < _corridors.Count; i++)
            {
                var corridor = _corridors.ElementAt(i);
                DungeonCorridor dungeonCorridor = DungeonCorridor.Create(corridor.Key, i);
                dungeonCorridor.Construct(_components, _dungeon.Parameter("corridorSize").Vector());
                dungeonCorridor.transform.SetParent(_dungeonTransform);
                _corridors[corridor.Key] = dungeonCorridor;
            }

            foreach (var room in _roomBounds.Values)
            {
                foreach (var corridor in _corridors.Values)
                {
                    room.Modify(corridor);
                    corridor.Modify(room.Bounds);
                }
            }
        }

        /// <summary>
        /// Places the content in each room, including the dungeon exit and player starting point.
        /// </summary>
        private void PlaceContent()
        {
            // Place dungeon exit point
            Bounds lastRoom = _roomBounds.Last().Key;
            DungeonExit exit = GameObject.Instantiate(_components.exit, lastRoom.center,
                Quaternion.identity, _dungeonTransform);
            exit.name = "DungeonExit";
            _dungeon.DungeonExit = exit;

            foreach (var room in _roomBounds.Values)
            {
                foreach (var content in room.Contents)
                {
                    for (int i = 0; i < content.Value; i++)
                    {
                        GameObject gameObject = GameObject.Instantiate(content.Key);
                        gameObject.transform.position += PointUtils.RandomPointWithinRange(room.Bounds);
                    }
                }
            }

            // Place player at start of dungeon
            Bounds firstRoom = _roomBounds.First().Key;
            SpawnPoint startingPoint = GameObject.Instantiate(_components.startingPoint, firstRoom.center,
                Quaternion.identity, _dungeonTransform);

            startingPoint.name = "DungeonEntrypoint";
            _dungeon.StartingPoint = startingPoint;
        }

        /// <summary>
        /// Creates a corridor between two rooms.
        /// </summary>
        /// <param name="b1">the bounds of the first room</param>
        /// <param name="b2">the bounds of the second room</param>
        /// <param name="dist">the distance between the two rooms</param>
        /// <param name="isHorizontal">is the corridor a horizontal connection or not</param>
        /// <returns>the bounds of the corridor</returns>
        private Bounds CreateCorridor(Bounds b1, Bounds b2, float dist, bool isHorizontal)
        {
            Vector3 corridorCenter;
            if (isHorizontal)
            {
                float zOffset = _dungeon.CorridorSize.z / 2f;
                corridorCenter = PointUtils.RandomPointWithinRange(new(b1.max.x, 0, Mathf.Max(b1.min.z, b2.min.z) + zOffset), new(b1.max.x, 0, Mathf.Min(b1.max.z, b2.max.z) - zOffset));
                corridorCenter.x += (dist / 2f);
            }
            else
            {
                float xOffset = _dungeon.CorridorSize.x / 2f;
                corridorCenter = PointUtils.RandomPointWithinRange(new(Mathf.Max(b1.min.x, b2.min.x) + xOffset, 0, b1.max.z), new(Mathf.Min(b1.max.x, b2.max.x) - xOffset, 0, b1.max.z));
                corridorCenter.z += (dist / 2f);
            }
            return new Bounds(corridorCenter, new(isHorizontal ? dist : _dungeon.CorridorSize.x, 0, isHorizontal ? _dungeon.CorridorSize.z : dist));
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
            Range<Vector3> roomSizeParam = _dungeon.Parameter("roomSize").VectorRange();
            float roomOffset = roomSizeParam.max.magnitude /2f; // Distance between rooms
            Vector3 roomSize = PointUtils.RandomSize(roomSizeParam.min, roomSizeParam.max);
            Vector3 roomCenter = PointUtils.RandomPointWithinRange(min, max);

            if (isHorizontal)
            {
                roomCenter.x += dir.x > 0 ? (Random.value * roomOffset) + roomSize.x : -((Random.value * roomOffset) + roomSize.x);
            }
            else
            {
                roomCenter.z += dir.z > 0 ? (Random.value * roomOffset) + roomSize.z : -((Random.value * roomOffset) + roomSize.z);
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
        /// <inheritdoc/>
        /// </summary>
        public void ClearDungeon()
        {
            _roomBounds?.Clear();
            _corridors?.Clear();

            _components = null;
            _dungeon = null;
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