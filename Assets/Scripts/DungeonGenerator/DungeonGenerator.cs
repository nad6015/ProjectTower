using UnityEngine;
using Assets.DungeonGenerator.Components;
using System.Collections.Generic;

namespace Assets.DungeonGenerator
{
    public class DungeonGenerator : MonoBehaviour
    {
        /// <summary>
        /// Generates a new dungeon using the provided dungeon parameters.
        /// </summary>
        /// <param name="representation">the parameters for the dungeon.</param>
        public Dungeon GenerateDungeon(DungeonRepresentation representation)
        {
            List<IDungeonAlgorithm> algorithms = new()
            {
                new GraphGrammar(),
                new RandomWalk(transform) 
            };
            algorithms.ForEach(algorithm => algorithm.GenerateDungeon(representation));
            return representation.GetConstructedDungeon();
        }

        public void ClearDungeon()
        {
            // Remove child object code copied from - https://stackoverflow.com/questions/46358717/how-to-loop-through-and-destroy-all-children-of-a-game-object-in-unity
            while (transform.childCount > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
            DungeonNode.Reset();
        }
    }
}