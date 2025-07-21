using UnityEngine;
using Assets.DungeonGenerator.Components;
using Unity.AI.Navigation;

namespace Assets.DungeonGenerator
{
    [RequireComponent(typeof(DungeonComponents))]
    public class DungeonGenerator : MonoBehaviour
    {
        private DungeonComponents _components;

        private readonly IDungeonAlgorithm _grammarDungeonGenerator;
        private readonly Dungeon _dungeon;

        private void Start()
        {
            // grammarDungeonGenerator = new MissionGrammarAlgorithm();
            _components = GetComponent<DungeonComponents>();
            _components.navMesh = GetComponent<NavMeshSurface>();
        }

        /// <summary>
        /// Generates a new dungeon using the provided dungeon parameters.
        /// </summary>
        /// <param name="parameters">the parameters for the dungeon.</param>
        public Dungeon GenerateDungeon(DungeonParameters parameters)
        {
            // TODO: Run dungeon parameters through misson grammar, then pass result to BSP algorithm
            // TODO: Might need to build navmesh before placing enemies
            Dungeon d = new Dungeon(parameters, _components);
            IDungeonAlgorithm grammarDungeonGenerator = new GraphGrammarAlgorithm(parameters, _components);
            grammarDungeonGenerator.GenerateDungeon();


            IDungeonAlgorithm algorithm = new BSPAlgorithm(d);
            algorithm.GenerateDungeon();
            return d;
        }
    }
}