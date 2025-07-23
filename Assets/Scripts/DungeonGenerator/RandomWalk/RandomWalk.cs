using UnityEngine;
using Assets.DungeonGenerator.Components;
using System.Collections.Generic;
using System.Linq;

namespace Assets.DungeonGenerator
{
    using Random = UnityEngine.Random;
    public class RandomWalk : IDungeonAlgorithm
    {
        private readonly Dictionary<Bounds, DungeonRoom> _roomBounds;
        private readonly Dictionary<Bounds, DungeonCorridor> _corridors;

        private readonly Dungeon _dungeon;
        private readonly DungeonComponents _components;
        private readonly Transform _dungeonTransform;
        private readonly int offset = 1;

        public RandomWalk(Dungeon dungeon, Transform transform)
        {
            _dungeon = dungeon;
            _components = dungeon.Components;
            _dungeonTransform = transform;
            _corridors = new();
            _roomBounds = new();
        }

        /// <summary>
        /// Generates the representation of rooms in a dungeon. The pseudocode for this algorithm is as follows:
        ///    1. Choose a random size within the min and max bounds(note: this could be a parameter)
        ///    2. Choose a random axis(verticle or horizontal) and divide along it(coords can be random)
        ///    3. Choose one of the two paritions then repeat steps 2 and 3 until min room bound is met.
        ///    4. Repeat for the other partition.
        /// </summary>
        /// 
        public void GenerateDungeon()
        {
            Vector3 dir = RandomDirection();

            Bounds lastRoom = RandomRoom(Vector3.zero, Vector3.zero, dir.x != 0);
            _roomBounds.Add(lastRoom, null);

            while (_roomBounds.Count < _dungeon.MaxRooms)
            {
                Bounds nextRoom;
                Bounds corridor;

                if (dir == Vector3.right)
                {
                    nextRoom = RandomRoom(new(lastRoom.max.x, 0, lastRoom.min.z), lastRoom.max, true);
                    corridor = CreateCorridor(lastRoom, nextRoom, nextRoom.min.x - lastRoom.max.x, true);
                }
                else //if (dir == Vector3.up)
                {
                    nextRoom = RandomRoom(new(lastRoom.min.x, 0, lastRoom.max.z), lastRoom.max, false);
                    corridor = CreateCorridor(lastRoom, nextRoom, nextRoom.min.z - lastRoom.max.z, false);
                }

                _corridors.Add(corridor, null);
                _roomBounds.Add(nextRoom, null);
                lastRoom = nextRoom;
                dir = RandomDirection();
            }

            ConstructDungeon();
        }

        private Bounds CreateCorridor(Bounds b1, Bounds b2, float dist, bool isHorizontal)
        {
            Vector3 corridorCenter;
            if (isHorizontal)
            {
                float zOffset = _dungeon.MinCorridorSize.y / 2f;
                corridorCenter = PointUtils.RandomPointWithinRange(new(b1.max.x, 0, Mathf.Max(b1.min.z, b2.min.z) + zOffset), new(b1.max.x, 0, Mathf.Min(b1.max.z, b2.max.z) - zOffset));
                corridorCenter.x += (dist / 2f);
            }
            else
            {
                float xOffset = _dungeon.MinCorridorSize.x / 2f;
                corridorCenter = PointUtils.RandomPointWithinRange(new(Mathf.Max(b1.min.x, b2.min.x) + xOffset, 0, b1.max.z), new(Mathf.Min(b1.max.x, b2.max.x) - xOffset, 0, b1.max.z));
                corridorCenter.z += (dist / 2f);
            }
            return new Bounds(corridorCenter, new(isHorizontal ? dist : _dungeon.MinCorridorSize.x, 0, isHorizontal ? _dungeon.MinCorridorSize.y : dist));
        }

        private Bounds RandomRoom(Vector3 min, Vector3 max, bool isHorizontal)
        {
            float roomOffset = _dungeon.MaxRoomSize.magnitude; // Distance between rooms
            Vector3 roomSize = PointUtils.RandomSize(_dungeon.MinRoomSize, _dungeon.MaxRoomSize);
            Vector3 roomCenter = PointUtils.RandomPointWithinRange(min, max);

            if (isHorizontal)
            {
                roomCenter.x += (Random.value * roomOffset) + roomSize.x;
            }
            else
            {
                roomCenter.z += (Random.value * roomOffset) + roomSize.z;
            }

            return new Bounds(roomCenter, roomSize);
        }


        private Vector3 RandomDirection()
        {
            if (_dungeon.Parameters.rootDungeonSplit >= Random.value)
            {
                return Vector3.right;
            }
            return Vector3.forward;
        }

        public void ConstructDungeon()
        {
            // Dictionary upsert code referenced from - https://stackoverflow.com/questions/1243717/how-to-update-the-value-stored-in-dictionary-in-c
            for (int i = 0; i < _roomBounds.Count; i++)
            {
                var room = _roomBounds.ElementAt(i);
                _roomBounds[room.Key] = DungeonRoom.Create(room.Key, i);
                _roomBounds[room.Key].Construct(_components.floorTile, _components.wallTile, null);
            }

            for (int i = 0; i < _corridors.Count; i++)
            {
                var corridor = _corridors.ElementAt(i);
                _corridors[corridor.Key] = DungeonCorridor.Create(corridor.Key, i);
                _corridors[corridor.Key].Construct(_components);
            }
        }


        private void PlaceContent()
        {
            // Place dungeon exit point
            Bounds lastRoom = _roomBounds.Last().Key;
            DungeonExit exit = GameObject.Instantiate(_components.exit, PointUtils.Vec2ToVec3(lastRoom.center), Quaternion.identity);
            exit.name = "DungeonExit";

            foreach (var room in _roomBounds)
            {
                PlaceContent(room.Value);
            }

            _components.navMesh.BuildNavMesh();

            // Generate navmesh
            // Place player at start of dungeon
            Bounds firstRoom = _roomBounds.First().Key;
            _components.startingPoint.Spawn(PointUtils.Vec2ToVec3(firstRoom.center));
        }

        private void PlaceContent(DungeonRoom room)
        {
            foreach (KeyValuePair<GameObject, int> content in room.Contents)
            {
                for (int i = 0; i < content.Value; i++)
                {
                    GameObject gameObject = GameObject.Instantiate(content.Key);
                    gameObject.transform.position += PointUtils.GetRandomPointWithinBounds(room.rectBounds);
                }
            }
        }
    }
}