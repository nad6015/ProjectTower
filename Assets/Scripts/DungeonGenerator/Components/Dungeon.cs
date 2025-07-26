using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DungeonGenerator.Components
{
    /// <summary>
    /// A representation of a dungeon. Has its paramters set by the graph grammar and its components by the random walk algorithm.
    /// </summary>
    public class Dungeon
    {
        public DungeonExit DungeonExit { get; internal set; }
        public SpawnPoint StartingPoint { get; internal set; }

        public DungeonAxis DungeonAxis { get; }
        public Vector2 Size { get { return Parameters.Size; } }
        public Vector2 MinRoomSize { get { return Parameters.MinRoomSize; } }
        public Vector2 MaxRoomSize { get { return Parameters.MaxRoomSize; } }
        public DungeonComponents Components { get; internal set; }
        public Vector2 MinCorridorSize { get { return Parameters.CorridorMinSize; } }

        internal DungeonParameters Parameters { get; private set; }
        internal Dictionary<string, DungeonRoom> DungeonRooms { get; }
        public int MaxRooms { get { return Parameters.MaxRooms; } }


        /// <summary>
        /// Constructs a new dungeon.
        /// </summary>
        /// <param name="parameters">The parameters for this dungeon.</param>
        public Dungeon(DungeonParameters parameters, DungeonComponents components)
        {
            Parameters = parameters;
            Components = components;
        }
    }
}