using UnityEngine;
using Assets.DungeonGenerator.Components;
using System.Collections.Generic;

namespace Assets.DungeonGenerator
{
    public class DungeonGenerator : MonoBehaviour
    {
        [SerializeField]
        private DungeonComponents _components;

        private List<IDungeonAlgorithm> _algorithms;

        private void Start()
        {
            _algorithms = new List<IDungeonAlgorithm>()
            {
                new GraphGrammar(),
                new RandomWalk(transform)
            };
        }

        /// <summary>
        /// Generates a new dungeon using the provided dungeon parameters.
        /// </summary>
        /// <param name="representation">the parameters for the dungeon.</param>
        public Dungeon GenerateDungeon(DungeonRepresentation representation)
        {
            representation.SetComponents(_components);
            _algorithms.ForEach(algorithm => algorithm.GenerateDungeon(representation));
            return representation.GetConstructedDungeon();
        }

        public void ClearDungeon()
        {
            _algorithms.ForEach(algorithm => algorithm.ClearDungeon());

            // Remove child object code copied from - https://stackoverflow.com/questions/46358717/how-to-loop-through-and-destroy-all-children-of-a-game-object-in-unity
            while (transform.childCount > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
            DungeonNode.Reset();
        }
    }
}