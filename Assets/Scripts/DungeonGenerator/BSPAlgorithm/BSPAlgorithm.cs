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
        private Graph<BSPNode> _connectedRooms = new();

        private readonly Dungeon _dungeon;
        private readonly DungeonComponents _components;
        private readonly int offset = 1;

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
            DungeonAxis axis = RandomAxis();
            _root = new BSPNode(_dungeon, axis);
            _rooms = new List<BSPNode>();

            PartitionSpace(_root, axis);

            if (_rooms.Count > 0)
            {
                ConnectRooms();
                ConstructDungeon();
                PlaceContent();
            }
        }

        private void ConnectRooms()
        {
            int j = 0;
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

                    ////bool isCloseVertically = Vector2.Distance(new(firstRoom.Bounds.x, firstRoom.Bounds.yMax), new(secondRoom.Bounds.x, secondRoom.Bounds.y)) < 
                    ////     (_dungeon.MaxRoomSize.y);

                    ////bool isCloseHorizontally = Vector2.Distance(new(firstRoom.Bounds.xMax, firstRoom.Bounds.y), new(secondRoom.Bounds.x, secondRoom.Bounds.y)) <
                    ////    (_dungeon.MaxRoomSize.x);

                    ////Debug.Log("rooms distance = " + Vector2.Distance(firstRoom.Bounds.max, secondRoom.Bounds.min));
                    ////Debug.Log("is close vertically = " + isCloseVertically);
                    ////Debug.Log("is close horizontally = " + isCloseHorizontally);

                    //if (!_connectedRooms.Contains(firstRoom) || !_connectedRooms[firstRoom].Contains(secondRoom))
                    //&& Vector2.Distance(firstRoom.Bounds.center, secondRoom.Bounds.center) < _dungeon.MinRoomSize.magnitude) 
                    //{
                    //Rect overlappingXBounds = new(firstRoom.Bounds.xMax, firstRoom.Bounds.y, secondRoom.Bounds.x - firstRoom.Bounds.x, firstRoom.Bounds.height);
                    //Rect overlappingNegativeXBounds = new Rect(firstRoom.Bounds.x, firstRoom.Bounds.y, secondRoom.Bounds.xMax - firstRoom.Bounds.x, firstRoom.Bounds.height);

                    ////Rect overlappingYBounds = new(secondRoom.Bounds.x, secondRoom.Bounds.yMax, secondRoom.Bounds.width, firstRoom.Bounds.y - secondRoom.Bounds.yMax);
                    //Rect overlappingYBounds = new(firstRoom.Bounds.x, firstRoom.Bounds.yMax, firstRoom.Bounds.width, secondRoom.Bounds.y - firstRoom.Bounds.y);
                    //Rect overlappingNegativeYBounds = new Rect(firstRoom.Bounds.x, firstRoom.Bounds.yMax, firstRoom.Bounds.width, secondRoom.Bounds.yMax - firstRoom.Bounds.y);

                    //if (overlappingYBounds.Overlaps(secondRoom.Bounds))
                    //{
                    //    //Debug.Log("firstRoom.Bounds = " + firstRoom.Bounds);
                    //    //Debug.Log("secondRoom.Bounds = " + secondRoom.Bounds);

                    //    //Debug.Log("overlappingYBounds = " + overlappingYBounds);

                    //    Debug.Log("Connecting via vertical corridor");
                    //    if (firstRoom.Bounds.y > secondRoom.Bounds.y)
                    //    {
                    //        Debug.Log("First room is placed further up than second room");

                    //        //secondRoom.ConnectTo(firstRoom, _dungeon.CorridorMinSize, DungeonAxis.VERTICAL);
                    //    }
                    //    else
                    //    {
                    //        Debug.Log("First room is placed further down than second room");
                    //        //firstRoom.ConnectTo(secondRoom, _dungeon.CorridorMinSize, DungeonAxis.VERTICAL);
                    //    }
                    //    _connectedRooms.Add(firstRoom, secondRoom);
                    //}
                    //else if (overlappingXBounds.Overlaps(secondRoom.Bounds))
                    //{
                    //    if (firstRoom.Bounds.x > secondRoom.Bounds.x)
                    //    {
                    //        Debug.Log("First room is placed further right than second room");
                    //        //   secondRoom.ConnectTo(firstRoom, _dungeon.CorridorMinSize, DungeonAxis.HORIZONTAL);
                    //    }
                    //    else
                    //    {
                    //        Debug.Log("First room is placed further left than second room");
                    //        // firstRoom.ConnectTo(secondRoom, _dungeon.CorridorMinSize, DungeonAxis.HORIZONTAL);
                    //    }
                    //    //Debug.Log("firstRoom.Bounds = " + firstRoom.Bounds);
                    //    //Debug.Log("secondRoom.Bounds = " + secondRoom.Bounds);

                    //    //Debug.Log("overlappingXBounds = " + overlappingXBounds);
                    //    //Debug.Log("Connecting via horizontal corridor");

                    //    _connectedRooms.Add(firstRoom, secondRoom);
                    //}
                    //else
                    //{
                    //    Debug.Log("Not adding to connected rooms");
                    //}
                }

                if (secondRoom == null)
                {
                    Debug.Log("nodes.count = " + nodes.Count);
                    for (int i = 0; i < _rooms.Count; i++)
                    {
                        BSPNode node = _rooms[i];
                        float dist = Vector2.Distance(firstRoom.Bounds.center, node.Bounds.center);

                        if (node == firstRoom || secondRoom == node)
                        {
                            continue;
                        }

                        if (dist < minDist && dist < _dungeon.MaxRoomSize.magnitude * 2)
                        {
                            minDist = dist;
                            secondRoom = node;
                        }
                    }

                    firstRoom.ConnectTo(secondRoom, _dungeon.MinCorridorSize, RandomAxis());
                    if (nodes.Contains(secondRoom))
                    {
                        nodes.Remove(secondRoom);
                        firstRoom = secondRoom;
                    }
                    else
                    {
                        nodes.Remove(firstRoom);
                        firstRoom = nodes[0];
                    }
                }
                else
                {
                    nodes.Remove(secondRoom);
                    firstRoom.ConnectTo(secondRoom, _dungeon.MinCorridorSize, RandomAxis());
                    firstRoom = secondRoom;
                }
            }

            HashSet<BSPNode> visitedNodes = new HashSet<BSPNode>();
            //foreach (var roomNode in rooms)
            //{
            //    BSPNode room = roomNode;
            //    roomNode.Value.ForEach(r =>
            //    {
            //        if (!visitedNodes.Contains(r))
            //        {
            //            visitedNodes.Add(r);
            //            room.ConnectTo(r, _dungeon.Parameters.CorridorMinSize);
            //        }
            //    });
            //    visitedNodes.Add(room);
            //}
            //    for (int i = 0; i < rooms.Count; i++)
            //    {
            //        BSPNode secondRoom = rooms[i];
            //        if (room.HasMaxCorridors() || roomNode.Value.Contains(secondRoom))
            //        {
            //            break;
            //        }

            //        Rect overlappingXBounds, overlappingYBounds, overlappingNegativeXBounds, overlappingNegativeYBounds;

            //        overlappingXBounds = new Rect(room.Bounds.xMax, room.Bounds.y, room.Bounds.xMax + secondRoom.Bounds.xMax, room.Bounds.height);
            //        overlappingYBounds = new Rect(room.Bounds.x, room.Bounds.yMax, room.Bounds.width, room.Bounds.yMax + secondRoom.Bounds.yMax);

            //        overlappingNegativeXBounds = new Rect(room.Bounds.x, room.Bounds.y, secondRoom.Bounds.xMax - room.Bounds.x, room.Bounds.height);
            //        overlappingNegativeYBounds = new Rect(room.Bounds.x, room.Bounds.y, room.Bounds.width, secondRoom.Bounds.yMax - room.Bounds.y);



            //        if (overlappingXBounds.Overlaps(secondRoom.Bounds, true) || overlappingYBounds.Overlaps(secondRoom.Bounds, true) || overlappingNegativeXBounds.Overlaps(secondRoom.Bounds, true) || overlappingNegativeYBounds.Overlaps(secondRoom.Bounds, false))
            //        {
            //            nodesToConnect.Add(new(room, secondRoom));
            //        }

            //    }
            //}

            //foreach (KeyValuePair<BSPNode, List<BSPNode>> rooms in _connectedRooms)
            //{
            //    rooms.Value.ForEach(r =>
            //    {
            //        rooms.Key.ConnectTo(r, _dungeon.CorridorMinSize);
            //    });
            //}

            /*
            *Generator corridor representation:
         *1.Starting from the bottom-left most parent node, choose a random axis and link the children nodes via a straight line.
         * 2.Repeat for other parent nodes at the same level.
         * 3.Repeat steps 1 and 2 until the root node is reached.
         * */
        }

        private void PartitionSpace(BSPNode node, DungeonAxis axis)
        {
            if(node == null)
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
            BSPNode left = new BSPNode(node, leftSpace, axis);
            BSPNode right = null;
            if (!shouldCreateOneNode)
            {
                Rect rightSpace = new(x, y, rightWidth - offset, rightHeight - offset);
                right = new BSPNode(node, rightSpace, axis);
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
                    secondNode?.GenerateRoom().Construct(_components.floorAsset, _components.wallAsset, null);
                }
            }

            foreach (var room in _rooms)
            {
                for (var i = 0; _corridors.Count > i; i++)
                {
                    DungeonCorridor corridor = _corridors[i];

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