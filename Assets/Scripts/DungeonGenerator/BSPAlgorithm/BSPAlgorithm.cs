using UnityEngine;
using Assets.DungeonGenerator.Components;
using System.Collections.Generic;
using System.Linq;

namespace Assets.DungeonGenerator
{
    using Random = UnityEngine.Random;
    public class BSPAlgorithm : IDungeonAlgorithm
    {
        private BSPNode _root;
        private List<BSPNode> _rooms;
        private readonly List<DungeonCorridor> _corridors = new();

        private readonly Dungeon _dungeon;
        private readonly DungeonComponents _components;
        private readonly Transform _dungeonTransform;
        private readonly int offset = 1;

        public BSPAlgorithm(Dungeon dungeon, Transform transform)
        {
            _dungeon = dungeon;
            _components = dungeon.Components;
            _dungeonTransform = transform;

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
            DungeonAxis axis = RandomAxis();
            _root = new BSPNode(_dungeon, axis);
            _rooms = new List<BSPNode>();

            PartitionSpace(_root, axis);

            ConnectRooms();
            ConstructDungeon();
            PlaceContent();
        }

        private void ConnectRooms()
        {
            List<BSPNode> nodes = new(_rooms);

            BSPNode firstRoom = nodes[0];
            nodes.Remove(firstRoom);
            while (nodes.Count > 0)
            {
                BSPNode secondRoom = null;
                float minDist = float.MaxValue;

                for (int i = 0; i < nodes.Count; i++)
                {
                    BSPNode room = nodes[i];
                    float dist = Vector2.Distance(firstRoom.Bounds.center, room.Bounds.center);

                    if (dist < minDist)
                    {
                        minDist = dist;
                        secondRoom = room;
                    }
                }

                nodes.Remove(secondRoom);
                firstRoom.ConnectTo(secondRoom, _dungeon.MinCorridorSize, RandomAxis());
                firstRoom = secondRoom;
            }

            /*
            *Generator corridor representation:
         *1.Starting from the bottom-left most parent node, choose a random axis and link the children nodes via a straight line.
         * 2.Repeat for other parent nodes at the same level.
         * 3.Repeat steps 1 and 2 until the root node is reached.
         * */
        }

        private void PartitionSpace(BSPNode node, DungeonAxis axis)
        {
            if (_rooms.Count >= _dungeon.MaxRooms)
            {
                return;
            }

            if (node == null)
            {
                return;
            }

            if (node.IsRoomMinSize())
            {
                _rooms.Add(node);
                return;
            }

            if (node.CanBeSplitHorizontally() && node.CanBeSplitVertically())
            {
                CreateNewNodes(node, RandomAxis());
            }
            else if (node.CanBeSplitVertically())
            {
                CreateNewNodes(node, DungeonAxis.VERTICAL);
            }
            else
            {
                CreateNewNodes(node, DungeonAxis.HORIZONTAL);
            }

            PartitionSpace(node.Left, axis);
            PartitionSpace(node.Right, axis);
        }

        private void CreateNewNodes(BSPNode node, DungeonAxis axis)
        {
            Rect nodeSize = node.Bounds;
            float x = nodeSize.x;
            float y = nodeSize.y;

            float width = nodeSize.width;
            float height = nodeSize.height;

            float rightWidth = nodeSize.width;
            float rightHeight = nodeSize.height;

            bool shouldCreateOneNode = nodeSize.width <= _dungeon.MaxRoomSize.x || nodeSize.height <= _dungeon.MaxRoomSize.y;

            if (axis == DungeonAxis.HORIZONTAL)
            {
                height = Mathf.Round(Random.Range(_dungeon.MinRoomSize.y, node.Bounds.height));
                rightHeight -= height;

                if (rightHeight < _dungeon.MinRoomSize.y)
                {
                    height -= _dungeon.MinRoomSize.y;
                    rightHeight = _dungeon.MinRoomSize.y;
                }

                if (height + rightHeight < nodeSize.height)
                {
                    height = nodeSize.height - rightHeight;
                }

                y += height;
                shouldCreateOneNode = height < _dungeon.MinRoomSize.y && rightHeight >= _dungeon.MinRoomSize.y;
            }
            else
            {
                width = Mathf.Round(Random.Range(_dungeon.MinRoomSize.x, node.Bounds.width));
                rightWidth -= width;
                if (rightWidth < _dungeon.MinRoomSize.x)
                {
                    width -= _dungeon.MinRoomSize.x;
                    rightWidth = _dungeon.MinRoomSize.x;
                }

                if (width + rightWidth < nodeSize.width)
                {
                    width = nodeSize.width - rightWidth;
                }

                x += width;
                shouldCreateOneNode = width < _dungeon.MinRoomSize.x && rightWidth >= _dungeon.MinRoomSize.x;
            }

            if (shouldCreateOneNode)
            {
                width = Mathf.Round(Random.Range(_dungeon.MinRoomSize.x, nodeSize.width));
                height = Mathf.Round(Random.Range(_dungeon.MinRoomSize.y, nodeSize.height));
            }

            Rect leftSpace = new(nodeSize.x, nodeSize.y, width - offset, height - offset);
            BSPNode left = new(node, leftSpace, axis);
            
            if (!shouldCreateOneNode)
            {
                Rect rightSpace = new(x, y, rightWidth - offset, rightHeight - offset);
                BSPNode bSPNode = new(node, rightSpace, axis);
            }
        }

        private DungeonAxis RandomAxis()
        {
            if (_dungeon.Parameters.rootDungeonSplit >= Random.value)
            {
                return DungeonAxis.VERTICAL;
            }
            return DungeonAxis.HORIZONTAL;
        }

        public void ConstructDungeon()
        {
            for (int i = 0; i < _rooms.Count; i++)
            {
                BSPNode firstNode = _rooms[i];
                BSPNode secondNode = i != _rooms.Count - 1 ? _rooms[i + 1] : null;

                _corridors.AddRange(firstNode.GenerateCorridors());
                if (!firstNode.HasRoom())
                {
                    ConstructRooms(firstNode, secondNode, null, _components);
                }
                else
                {
                    DungeonRoom room = secondNode?.GenerateRoom();
                    room?.transform.SetParent(_dungeonTransform);
                    room?.Construct(_components.floorTile, _components.wallTile, null);
                }
            }

            foreach (var room in _rooms)
            {
                for (var i = 0; _corridors.Count > i; i++)
                {
                    DungeonCorridor corridor = _corridors[i];
                    corridor.transform.SetParent(_dungeonTransform);

                    corridor.Construct(_components);
                    room?.Room.Modify(corridor);
                }
            }
        }

        private void ConstructRooms(BSPNode firstNode, BSPNode secondNode, DungeonCorridor corridor, DungeonComponents components)
        {
            if (firstNode.IsRoomMinSize() && !firstNode.HasRoom())
            {
                DungeonRoom room = firstNode.GenerateRoom();
                room.transform.SetParent(_dungeonTransform);
                room.Construct(components.floorTile, components.wallTile, corridor);
            }

            if (secondNode != null && secondNode.IsRoomMinSize() && !secondNode.HasRoom())
            {
                DungeonRoom room = secondNode.GenerateRoom();
                room.transform.SetParent(_dungeonTransform);
                room.Construct(components.floorTile, components.wallTile, corridor);
            }
        }

        private void PlaceContent()
        {
            // Place dungeon exit point
            BSPNode lastRoom = _rooms.Last();
            DungeonExit exit = GameObject.Instantiate(_components.exit, DungeonGeneratorUtils.Vec2ToVec3(lastRoom.Bounds.center), Quaternion.identity);
            exit.name = "DungeonExit";

            for (var i = 0; i < _rooms.Count; i++)
            {
                PlaceContent(_rooms[i].Room);
            }

            _components.navMesh.BuildNavMesh();

            // Generate navmesh
            // Place player at start of dungeon
            BSPNode firstRoom = _rooms.First();
            _components.startingPoint.Spawn(DungeonGeneratorUtils.Vec2ToVec3(firstRoom.Bounds.center));
        }

        private void PlaceContent(DungeonRoom room)
        {
            foreach (KeyValuePair<GameObject, int> content in room.Contents)
            {
                for (int i = 0; i < content.Value; i++)
                {
                    GameObject gameObject = GameObject.Instantiate(content.Key);
                    gameObject.transform.position += DungeonGeneratorUtils.GetRandomPointWithinBounds(room.Bounds);
                }
            }
        }
    }
}