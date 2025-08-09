using Assets.Scripts.DungeonGenerator.Components;
using Assets.Scripts.DungeonGenerator.DataStructures;
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
        public DungeonComponents Components { get; private set; }
        public Vector3 CorridorSize { get { return Parameter("corridorSize").Vector(); } }

        private readonly DungeonParameters _parameters;
        internal Dictionary<string, DungeonRoom> DungeonRooms { get; }
        public DungeonLayout Rooms { get; private set; }
        public DungeonFlow Flow { get { return _parameters.GetLayout(); } }

        /// <summary>
        /// Constructs a new dungeon.
        /// </summary>
        /// <param name="parameters">The parameters for this dungeon.</param>
        public Dungeon(DungeonParameters parameters, DungeonComponents components)
        {
            _parameters = parameters;
            Components = components;
        }

        internal DungeonParameter Parameter(string name)
        {
            return _parameters.GetParameter(name);
        }

        internal void SetRooms(DungeonLayout dungeonRooms)
        {
            Rooms = dungeonRooms;
        }
    }
}