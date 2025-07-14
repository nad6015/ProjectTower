using UnityEngine;
using Assets.DungeonGenerator.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.DungeonGenerator
{
    using Random = UnityEngine.Random;
    public class BSPAlgorithm : IDungeonAlgorithm
    {
        private BSPNode bspTreeRoot;
        private List<BSPNode> bspTree;
        private List<BSPNode> rooms;
        private readonly List<DungeonCorridor> _corridors = new();
        private Graph<BSPNode> connectedRooms = new();

        private readonly Dungeon _dungeon;
        private readonly DungeonComponents _components;

        public BSPAlgorithm(Dungeon dungeon) 
        {
            this._dungeon = dungeon;
            this._components = dungeon.Components;
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
            bspTree = new List<BSPNode>();

            DungeonAxis axis = RandomAxis();
            bspTreeRoot = new BSPNode(_dungeon, axis);
            rooms = new List<BSPNode>();

            PartitionSpace(bspTreeRoot, axis);

            if (rooms.Count > 0)
            {
                ConnectRooms();
                ConstructDungeon();
                PlaceContent();
            }
        }

        private void ConnectRooms()
        {
            ConnectRooms(connectedRooms);
        }

        private void ConnectRooms(Graph<BSPNode> connectedRooms)
        {
            List<Tuple<BSPNode, BSPNode>> roomToConnect = new();
            List<BSPNode> roomsWithoutConnections = new();

            for (int i = 0; i < rooms.Count; i++)
            {
                BSPNode firstRoom = rooms[i];
                BSPNode secondRoom = i != rooms.Count - 1 ? rooms[i + 1] : null;
                if (secondRoom == null)
                {
                    if ((rooms.Count % 2 == 1))
                    {
                        roomsWithoutConnections.Add(firstRoom);
                    }
                    continue;
                }
                if (firstRoom.IsRoomMinSize())
                {
                    Rect overlappingXBounds = new(firstRoom.Bounds.width, firstRoom.Bounds.y, firstRoom.Bounds.xMax + secondRoom.Bounds.xMax, firstRoom.Bounds.height);

                    Rect overlappingYBounds = new(firstRoom.Bounds.x, firstRoom.Bounds.height, firstRoom.Bounds.width, firstRoom.Bounds.yMax + secondRoom.Bounds.yMax);

                    if (overlappingXBounds.Overlaps(secondRoom.Bounds) || overlappingYBounds.Overlaps(secondRoom.Bounds))
                    {
                        connectedRooms.Add(firstRoom, secondRoom);
                    }
                    else
                    {
                        connectedRooms.Add(firstRoom);
                    }
                }
            }

            foreach (var roomNode in connectedRooms)
            {
                BSPNode room = roomNode.Key;
                if (room == rooms[0] || room == rooms[^1])
                {
                    continue;
                }
                for (int i = 0; i < rooms.Count; i++)
                {
                    BSPNode secondRoom = rooms[i];
                    if (room.HasMaxCorridors() || roomNode.Value.Contains(secondRoom))
                    {
                        break;
                    }

                    Rect overlappingXBounds, overlappingYBounds, overlappingNegativeXBounds, overlappingNegativeYBounds;

                    overlappingXBounds = new Rect(room.Bounds.xMax, room.Bounds.y, room.Bounds.xMax + secondRoom.Bounds.xMax, room.Bounds.height);
                    overlappingYBounds = new Rect(room.Bounds.x, room.Bounds.yMax, room.Bounds.width, room.Bounds.yMax + secondRoom.Bounds.yMax);

                    overlappingNegativeXBounds = new Rect(room.Bounds.x, room.Bounds.y, secondRoom.Bounds.xMax - room.Bounds.x, room.Bounds.height);
                    overlappingNegativeYBounds = new Rect(room.Bounds.x, room.Bounds.y, room.Bounds.width, secondRoom.Bounds.yMax - room.Bounds.y);



                    if (overlappingXBounds.Overlaps(secondRoom.Bounds, true) || overlappingYBounds.Overlaps(secondRoom.Bounds, true) || overlappingNegativeXBounds.Overlaps(secondRoom.Bounds, true) || overlappingNegativeYBounds.Overlaps(secondRoom.Bounds, false))
                    {
                        roomToConnect.Add(new(room, secondRoom));
                    }

                }
            }

            foreach (KeyValuePair<BSPNode, List<BSPNode>> rooms in connectedRooms)
            {
                rooms.Value.ForEach(r =>
                {
                    rooms.Key.ConnectTo(r, _dungeon.CorridorMinSize);
                });
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
            bspTree.Add(node);
            if (node.IsRoomMinSize())
            {
                rooms.Add(node);
                return;
            }
            else
            {
                CreateNewNodes(node, axis);

                DungeonAxis childNodeAxis = RandomAxis();

                PartitionSpace(node.Left, childNodeAxis);

                if (node.Right != null)
                {
                    PartitionSpace(node.Right, childNodeAxis);
                }
            }
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

            bool shouldCreateOneNode = false;

            if (axis == DungeonAxis.HORIZONTAL)
            {
                height = Mathf.Round(Random.Range(_dungeon.RoomMinSize.y, node.Bounds.height));
                rightHeight -= height;

                if (rightHeight < _dungeon.RoomMinSize.y)
                {
                    height -= _dungeon.RoomMinSize.y;
                    rightHeight = _dungeon.RoomMinSize.y;
                }

                if (height + rightHeight < nodeSize.height)
                {
                    height = nodeSize.height - rightHeight;
                }

                y += height;
                shouldCreateOneNode = height < _dungeon.RoomMinSize.y && rightHeight >= _dungeon.RoomMinSize.y;
            }
            else
            {
                width = Mathf.Round(Random.Range(_dungeon.RoomMinSize.x, node.Bounds.width));
                rightWidth -= width;
                if (rightWidth < _dungeon.RoomMinSize.x)
                {
                    width -= _dungeon.RoomMinSize.x;
                    rightWidth = _dungeon.RoomMinSize.x;
                }

                if (width + rightWidth < nodeSize.width)
                {
                    width = nodeSize.width - rightWidth;
                }

                x += width;
                shouldCreateOneNode = width < _dungeon.RoomMinSize.x && rightWidth >= _dungeon.RoomMinSize.x;
            }

            if (shouldCreateOneNode)
            {
                width = Mathf.Round(Random.Range(_dungeon.RoomMinSize.x, nodeSize.width - 1));
                height = Mathf.Round(Random.Range(_dungeon.RoomMinSize.y, nodeSize.height - 1));
            }

            Rect leftSpace = new(nodeSize.x, nodeSize.y, width - 2, height - 2);
            new BSPNode(node, leftSpace, axis);

            if (!shouldCreateOneNode)
            {
                Rect rightSpace = new(x, y, rightWidth - 2, rightHeight - 2);
                new BSPNode(node, rightSpace, axis);
            }
        }

        // TODO: Should be apart of DungeonMaster
        private DungeonAxis RandomAxis()
        {
            // TODO
            return DungeonAxis.HORIZONTAL;
        }

        public void ConstructDungeon()
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                BSPNode firstNode = rooms[i];
                BSPNode secondNode = i != rooms.Count - 1 ? rooms[i + 1] : null;

                if (firstNode.IsRoomMinSize())
                {

                    _corridors.AddRange(firstNode.GenerateCorridors());
                    if (!firstNode.HasRoom())
                    {
                        ConstructRooms(firstNode, secondNode, null, _components);
                    }
                    else
                    {
                        secondNode?.GenerateRoom().Construct(_components.floorAsset, _components.wallAsset, null);
                    }
                };
            }

            foreach (var room in rooms)
            {
                for (var i = 0; _corridors.Count > i; i++)
                {
                    _corridors[i].Construct(_components.corridorAsset, _components.floorAsset);
                    room?.Room.Modify(_corridors[i]);
                }
            }
        }

        private void ConstructRooms(BSPNode firstNode, BSPNode secondNode, DungeonCorridor corridor, DungeonComponents components)
        {
            if (firstNode.IsRoomMinSize() && !firstNode.HasRoom())
            {
                DungeonRoom room = firstNode.GenerateRoom();
                room.Construct(components.floorAsset, components.wallAsset, corridor);
            }

            if (secondNode != null && secondNode.IsRoomMinSize() && !secondNode.HasRoom())
            {
                DungeonRoom room = secondNode.GenerateRoom();
                room.Construct(components.floorAsset, components.wallAsset, corridor);
            }
        }

        private void PlaceContent()
        {
            // Place dungeon exit point
            BSPNode lastRoom = rooms.Last();
            DungeonExit exit = GameObject.Instantiate(_components.exit, DungeonGeneratorUtils.Vec2ToVec3(lastRoom.Bounds.center), Quaternion.identity);
            exit.name = "DungeonExit";

            for (var i = 0; i < rooms.Count; i++)
            {
                PlaceContent(rooms[i].Room);
            }

            _components.navMesh.BuildNavMesh();

            // Generate navmesh
            // Place player at start of dungeon
            BSPNode firstRoom = rooms.First();
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