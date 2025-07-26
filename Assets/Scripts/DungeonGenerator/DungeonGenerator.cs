using UnityEngine;
using Assets.DungeonGenerator.Components;
using Unity.AI.Navigation;
using System;

namespace Assets.DungeonGenerator
{
    public class DungeonGenerator : MonoBehaviour
    {
        [SerializeField]
        private DungeonComponents _components;

        private readonly IDungeonAlgorithm _grammarDungeonGenerator;
        private readonly Dungeon _dungeon;

        private void Start()
        {
            // grammarDungeonGenerator = new MissionGrammarAlgorithm();
        }

        /// <summary>
        /// Generates a new dungeon using the provided dungeon parameters.
        /// </summary>
        /// <param name="parameters">the parameters for the dungeon.</param>
        public Dungeon GenerateDungeon(DungeonParameters parameters)
        {
            // TODO: Run dungeon parameters through misson grammar, then pass result to Random Walk algorithm
            // TODO: Might need to build navmesh before placing enemies
            Dungeon d = new(parameters, _components);

            IDungeonAlgorithm grammarDungeonGenerator = new GraphGrammarAlgorithm(parameters, _components);
            grammarDungeonGenerator.GenerateDungeon();


            IDungeonAlgorithm algorithm = new RandomWalk(d, transform);
            algorithm.GenerateDungeon();
            GetComponent<NavMeshSurface>().BuildNavMesh();
            return d;
        }

        public void ClearDungeon()
        {
            // Remove child object code copied from - https://stackoverflow.com/questions/46358717/how-to-loop-through-and-destroy-all-children-of-a-game-object-in-unity
            while (transform.childCount > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
        }
    }
}