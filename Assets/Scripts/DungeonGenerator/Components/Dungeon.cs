using Assets.Combat;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DungeonGenerator.Components
{
    /// <summary>
    /// Holds all the information about a dungeon such as its rooms, the items and enemies within, the dungeon's theme, etc.
    /// </summary>
    public class Dungeon
    {
        public List<DungeonRoom> DungeonRooms { get; internal set; }
        public List<Enemy> Enemies { get; internal set; }

        /// <summary>
        /// Constructs a new dungeon.
        /// </summary>
        public Dungeon()
        {
            DungeonRooms = new();
        }
    }
}