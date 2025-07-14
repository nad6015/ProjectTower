using UnityEngine;
using Assets.DungeonGenerator.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.DungeonGenerator
{
    using Random = UnityEngine.Random;
    public class GraphGrammarAlgorithm : IDungeonAlgorithm
    {
        public Dungeon Dungeon { get; private set; }
        private Graph<DungeonRoom> _dungeonRooms = new();


        /// <summary>
        /// Constructs a new GraphGrammar Algorithm.
        /// </summary>
        /// <param name="parameters"></param>
        public GraphGrammarAlgorithm(DungeonParameters parameters, DungeonComponents components)
        {
            // TODO
            Dungeon = new Dungeon(parameters, components);
        }

        /// <summary>
        /// Generates a representation of dungeon using the given parameters.
        /// </summary>
        /// <param name="parameters"></param>
        public void GenerateDungeon()
        {
            // TODO
        }
    }
}