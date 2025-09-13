using System;
using UnityEngine;

namespace Assets.DungeonGenerator.Components
{
    public struct DungeonNodeLink
    {
        public Tuple<DungeonNode, DungeonNode> ConnectedRooms; 
        public Bounds Bounds;
        public DungeonAxis Axis;

        public DungeonNodeLink(Bounds bound, Tuple<DungeonNode, DungeonNode> connectedRooms, DungeonAxis axis)
        {
            ConnectedRooms = connectedRooms;
            Bounds = bound;
            Axis = axis;
        }
    }
}