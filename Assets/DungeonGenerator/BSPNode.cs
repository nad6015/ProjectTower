using Assets.DungeonGenerator.Components;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.DungeonGenerator
{
    internal class BSPNode
    {
        internal BSPNode Left { get; set; }
        internal BSPNode Right { get; set; }
        internal BSPNode Parent { get; private set; }
        internal Rect Bounds { get; set; }
        List<DungeonCorridor> Corridors { get; set; }

        public BSPNode(Dungeon dungeon)
        {
            Bounds = new Rect(Vector2.zero, dungeon.Size);
            Parent = null;
        }

        public BSPNode(BSPNode parent, Rect bounds)
        {
            Bounds = bounds;

            if (parent.Left == null)
            {
                Parent = parent;
                Parent.Left = this;
            }
            else if (parent.Right == null) { 
                Parent = parent;
                Parent.Right = this;
            }
            else {
                throw new ArgumentException("Parent already has two children.");
            }
        }

        internal DungeonRoom GenerateRoom()
        {   
            return DungeonRoom.Create(Bounds);
        }

        internal bool IsRoomMinSize(Vector2 minSize)
        {
            Debug.Log("Comparing bounds " + Bounds + " to minSize " + minSize);
            
            return minSize.x >= Bounds.width || minSize.y >= Bounds.height;
        }
    }
}
