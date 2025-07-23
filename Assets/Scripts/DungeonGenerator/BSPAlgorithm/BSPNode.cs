using Assets.DungeonGenerator.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.DungeonGenerator
{
    using Random = UnityEngine.Random;
    public class BSPNode
    {
        static int nodeId = 0;
        internal BSPNode Left { get; set; }
        internal BSPNode Right { get; set; }
        internal BSPNode Parent { get; private set; }
        internal Rect Bounds { get; set; }
        public DungeonRoom Room { get { return room; } }

        internal Dungeon dungeon;

        private readonly List<Tuple<Rect, DungeonAxis>> corridors = new();

        private readonly DungeonAxis axis;

        private DungeonRoom room;
        private bool hasRoom = false;
        public string name;

        public BSPNode(Dungeon dungeon, DungeonAxis axis)
        {
            this.dungeon = dungeon;
            Bounds = new Rect(Vector2.zero, dungeon.Size);
            this.axis = axis;
            Parent = null;
            name = "root";
        }

        public BSPNode(BSPNode parent, Rect bounds, DungeonAxis axis)
        {
            if (parent.Left == null)
            {
                Parent = parent;
                Parent.Left = this;
            }
            else if (parent.Right == null)
            {
                Parent = parent;
                Parent.Right = this;
            }
            else
            {
                throw new ArgumentException("Parent already has two children.");
            }

            Bounds = bounds;
            this.axis = axis;
            dungeon = parent.dungeon;
            name = "node";
        }

        internal DungeonRoom GenerateRoom()
        {
            if (hasRoom)
            {
                return room;
            }

            hasRoom = true;
            room = DungeonRoom.Create(Bounds, name);
            return room;
        }

        internal List<DungeonCorridor> GenerateCorridors()
        {
            List<DungeonCorridor> dc = new()
            {
                DungeonCorridor.Create(corridors)
            };
            return dc;
        }

        internal bool IsRoomMinSize()
        {
            bool isMinSize = (dungeon.MinRoomSize.x >= Bounds.width || dungeon.MinRoomSize.y >= Bounds.height)
                && (dungeon.MaxRoomSize.x >= Bounds.width && dungeon.MaxRoomSize.y >= Bounds.height);

            if (isMinSize && name.Equals("node"))
            {
                name = "Room " + nodeId++;
            }
            return isMinSize;
        }

        internal bool CanBeSplitHorizontally()
        {
            return dungeon.MinRoomSize.x * 2 <= Bounds.width;
        }

        internal bool CanBeSplitVertically()
        {
            return dungeon.MinRoomSize.y * 2 <= Bounds.height;
        }

        float RandomY(Rect r1, Rect r2)
        {
            float minY = math.max(r1.y, r2.y);
            float maxY = math.min(r1.yMax, r2.yMax);
            return Random.Range(minY, maxY);
        }

        float RandomX(Rect r1, Rect r2)
        {
            float minX = math.max(r1.x, r2.x);
            float maxX = math.min(r1.xMax, r2.xMax);
            return Random.Range(minX, maxX);
        }

        Tuple<Rect, DungeonAxis> CreateHorizontalCorridor(BSPNode node1, BSPNode node2, Vector2 minCorridorSize)
        {
            float y = RandomY(node1.Bounds, node2.Bounds) - minCorridorSize.y;
            float x = node1.Bounds.xMax;

            float width = math.abs(math.round(node2.Bounds.x - x));
            float height = minCorridorSize.y;

            Rect corridorBounds = new(x, y < node1.Bounds.y ? node1.Bounds.y : y, width, height);

            return new(corridorBounds, DungeonAxis.HORIZONTAL);
        }

        Tuple<Rect, DungeonAxis> CreateVerticalCorridor(BSPNode node1, BSPNode node2, Vector2 minCorridorSize)
        {
            float x = RandomX(node1.Bounds, node2.Bounds) - minCorridorSize.x;
            float y = node1.Bounds.yMax;

            float width = minCorridorSize.x;
            float height = math.abs(math.round(node2.Bounds.y - y));

            Rect corridorBounds = new(x < node1.Bounds.x ? node1.Bounds.x : x, y, width, height);

            return new(corridorBounds, DungeonAxis.VERTICAL);
        }

        internal void ConnectTo(BSPNode node, Vector2 minCorridorSize, DungeonAxis axis)
        {
            Rect firstRoom = Bounds;
            Rect secondRoom = node.Bounds;

            Vector2 dir = (firstRoom.center - secondRoom.center).normalized;
            float angle = Mathf.Round(Vector2.SignedAngle(Bounds.center, node.Bounds.center));

            Debug.Log("center of " + name + " = " + Bounds.center);
            Debug.Log("center of " + node.name + " = " + node.Bounds.center);
            

            Vector2 roundedDir = new Vector2(Mathf.Round(dir.x), Mathf.Round(dir.y));

            Debug.Log(roundedDir);

            GenerateCorridor(this, node, minCorridorSize, roundedDir);

            if (IsWithinBounds(this, node, minCorridorSize, roundedDir))
            {
                return;
            }

            if (roundedDir.x == 1 || roundedDir.x == -1)
            {
                GenerateNextCorridor(this, node, minCorridorSize, new Vector2(0, dir.y > 0 ? 1 : -1));
            }
            else
            {
                GenerateNextCorridor(this, node, minCorridorSize, new Vector2(dir.x > 0 ? 1 : -1, 0));
            }
            //    var corridor = corridors.Last();
            //    if (Mathf.Approximately(roundedDir.y, 0))
            //    {
            //        Debug.Log("dir.y = " + dir.y);
            //        if (roundedDir.x > 0 && (corridor.Item1.yMax < secondRoom.y))
            //        {
            //            GenerateCorridor(this, node, minCorridorSize, Vector2.up);
            //        }
            //        else if (roundedDir.x < 0 && (corridor.Item1.yMax < secondRoom.y))
            //        {
            //            GenerateCorridor(this, node, minCorridorSize, Vector2.up);
            //        }
            //        else if (roundedDir.x > 0 && (corridor.Item1.y > secondRoom.yMax))
            //        {
            //            GenerateCorridor(this, node, minCorridorSize, Vector2.down);
            //        }
            //        else if (roundedDir.x < 0 && (corridor.Item1.y > secondRoom.y))
            //        {
            //            GenerateCorridor(this, node, minCorridorSize, Vector2.down);
            //        }
            //    }
            //    // (dir.x >= 0.5f || dir.x <= -0.5f) && (dir.y >= 0.5f || dir.y <= -0.5f)
            //    if (dot > 0 && dot < 0.5)//(angle > 30f || angle < -30f))
            //    {
            //        GenerateJointCorridor(node, minCorridorSize, new(dir.x < 0 ? -1 : 1, dir.y < 0 ? -1 : 1));
            //        Rect verticalCorridor = corridors.Last().Item1;
            //        Rect horizontalCorridor = corridors[corridors.IndexOf(corridors.Last()) - 1].Item1;
            //        Debug.Log("horizontal corridor after: " + horizontalCorridor);
            //        Debug.Log("vertical corridor after: " + verticalCorridor);
            //    }
            //    else
            //    {

            //    }
            //}
        }

        private bool IsWithinBounds(BSPNode bSPNode, BSPNode node, Vector2 minCorridorSize, Vector2 roundedDir)
        {
            Rect corridor = corridors.Last().Item1;

            if (roundedDir.x > 0 || roundedDir.x < 0)
            {
                return corridor.y >= Math.Max(node.Bounds.y, bSPNode.Bounds.y) && corridor.y < Math.Min(node.Bounds.yMax, bSPNode.Bounds.yMax);
            }
            else if(roundedDir.y > 0 || roundedDir.y < 0)
            {
                return corridor.x >= Math.Max(node.Bounds.x, bSPNode.Bounds.x) && corridor.x < Math.Min(node.Bounds.xMax, bSPNode.Bounds.xMax);
            }

            return true;
        }

        private void GenerateCorridor(BSPNode node1, BSPNode node2, Vector2 minCorridorSize, Vector2 dir)
        {
            Tuple<Rect, DungeonAxis> corridor;

            if (Vector2.left.x == dir.x)
            {
                corridor = CreateHorizontalCorridor(node1, node2, minCorridorSize);
            }
            else if (Vector2.right.x == dir.x)
            {
                corridor = CreateHorizontalCorridor(node2, node1, minCorridorSize);
            }
            else if (Vector2.up.y == dir.y)
            {
                corridor = CreateVerticalCorridor(node2, node1, minCorridorSize);
            }
            else //if (Vector2.down == dir)
            {
                corridor = CreateVerticalCorridor(node1, node2, minCorridorSize);
            }
            corridors.Add(corridor);
        }

        private void GenerateNextCorridor(BSPNode node1, BSPNode node2, Vector2 minCorridorSize, Vector2 dir)
        {
            Tuple<Rect, DungeonAxis> corridor = corridors.Last();
            Debug.Log(corridor.Item1);

            Rect rect;
            DungeonAxis axis = Mathf.Approximately(dir.y, 0) ? DungeonAxis.HORIZONTAL : DungeonAxis.VERTICAL;

            if (Vector2.left == dir)
            {
                rect = new(corridor.Item1.xMax, corridor.Item1.yMax, node2.Bounds.x - corridor.Item1.xMax, minCorridorSize.y);
                Debug.Log("next corridor starts from left " + rect);
            }
            else if (Vector2.right == dir)
            {
                rect = new(node2.Bounds.xMax, corridor.Item1.yMax, node2.Bounds.xMax - node1.Bounds.x, minCorridorSize.y);
                
                Debug.Log("next corridor starts from right " + rect);
            }
            else if (Vector2.up == dir)
            {
                rect = new(corridor.Item1.xMax, node2.Bounds.yMax, minCorridorSize.x, corridor.Item1.yMax - node2.Bounds.yMax);
                Debug.Log("next corridor starts from up " + rect);
            }
            else //if (Vector2.down == dir)
            {
                rect = new(corridor.Item1.xMax, node1.Bounds.yMax, minCorridorSize.x, corridor.Item1.yMax - node1 .Bounds.yMax);
                Debug.Log("next corridor starts from down " + rect);
            }
            corridors.Add(new(rect, axis));
        }


        private void GenerateJointCorridor(BSPNode node, Vector2 minCorridorSize, Vector2 dir)
        {

            GenerateCorridor(this, node, minCorridorSize, new Vector2(dir.x, 0));
            Rect horizontalCorridor = corridors.Last().Item1;

            GenerateCorridor(this, node, minCorridorSize, new Vector2(0, dir.y));
            Rect verticalCorridor = corridors.Last().Item1;

            Debug.Log("horizontal corridor before: " + horizontalCorridor);
            Debug.Log("vertical corridor before: " + verticalCorridor);

            verticalCorridor.yMax = horizontalCorridor.y;
            horizontalCorridor.width += verticalCorridor.width;
        }

        internal bool HasRoom()
        {
            return hasRoom;
        }
    }
}
