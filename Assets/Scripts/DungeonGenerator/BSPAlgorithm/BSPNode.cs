using Assets.DungeonGenerator.Components;
using System;
using System.Collections.Generic;
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

        private List<Tuple<Rect, DungeonAxis>> corridors = new();

        private readonly DungeonAxis axis;
        
        private DungeonRoom room;
        private bool hasRoom = false;
        private string name; // TODO: This is for testing purposes.

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
            name = parent.name + ".node" + nodeId++;
        }

        internal DungeonRoom GenerateRoom()
        {
            if (hasRoom)
            {
                return room;
            }

            hasRoom = true;
            room = DungeonRoom.Create(Bounds);
            return room;
        }

        internal List<DungeonCorridor> GenerateCorridors()
        {
            List<DungeonCorridor> dc = new();
            foreach (Tuple<Rect, DungeonAxis> corridor in corridors)
            {
                if (corridors != null)
                {
                    dc.Add(DungeonCorridor.Create(corridor.Item1, corridor.Item2));
                }
            }
            return dc;
        }

        internal bool IsRoomMinSize()
        {
            return dungeon.RoomMinSize.x >= Bounds.width || dungeon.RoomMinSize.y >= Bounds.height;
        }

        private void GenerateCorridor(BSPNode firstRoom, BSPNode secondRoom, Vector2 minCorridorSize)
        {
            Vector2 position, position2, size;
            DungeonAxis axis;

            Rect overlappingBounds = new(firstRoom.Bounds.width, firstRoom.Bounds.y, firstRoom.Bounds.xMax + secondRoom.Bounds.xMax, firstRoom.Bounds.height);
            if (overlappingBounds.Overlaps(secondRoom.Bounds))
            {
                axis = DungeonAxis.HORIZONTAL;
            }
            else
            {
                axis = DungeonAxis.VERTICAL;
            }

            if (axis == DungeonAxis.VERTICAL)
            {
                float minX = math.max(firstRoom.Bounds.x, secondRoom.Bounds.x);
                float maxX = math.min(firstRoom.Bounds.xMax, secondRoom.Bounds.xMax);
                float x = Random.Range(minX, maxX - minCorridorSize.x);
                float y = firstRoom.Bounds.yMax;

                float width = minCorridorSize.x;
                float height = math.abs(math.round(secondRoom.Bounds.y - y));

                if (x + width > maxX)
                {
                    width = maxX - x;
                }

                position = new Vector2(math.round(x), y);
                position2 = new Vector2(math.round(x), secondRoom.Bounds.y);
                size = new Vector2(width, height);
                axis = DungeonAxis.VERTICAL;
            }
            else
            {
                float minY = math.max(firstRoom.Bounds.y, secondRoom.Bounds.y);
                float maxY = math.min(firstRoom.Bounds.yMax, secondRoom.Bounds.yMax);

                float x = firstRoom.Bounds.xMax;
                float y = math.round(Random.Range(minY, maxY - minCorridorSize.y));

                float width = math.abs(math.round(secondRoom.Bounds.x - x));
                float height = minCorridorSize.y;

                if (y + height > maxY)
                {
                    height -= y;
                }

                position = new Vector2(x, math.round(y));
                position2 = new Vector2(secondRoom.Bounds.x, math.round(y));
                size = new Vector2(width, height);
            }
            Rect corridorBounds = new Rect(position, size);

            bool dontCreate = false;
            for (int i = 0; i < corridors.Count; i++)
            {
                if (corridors[i].Item1.Overlaps(corridorBounds))
                {
                    dontCreate = true;
                    break;
                }
            }

            if (!dontCreate)
            {
                corridors.Add(new Tuple<Rect, DungeonAxis>(corridorBounds, axis));
            }
        }

        internal void ConnectTo(BSPNode item2, Vector2 minCorridorSize)
        {
            if (item2 != null && corridors.Count < 2)
            {
                GenerateCorridor(this, item2, minCorridorSize);
            }
        }

        internal bool HasRoom()
        {
            return hasRoom;
        }

        internal bool HasMaxCorridors()
        {
            return corridors.Count > 1;
        }
    }
}
