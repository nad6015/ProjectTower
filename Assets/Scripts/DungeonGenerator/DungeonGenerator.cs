using UnityEngine;
using Assets.DungeonGenerator.Components;
using Unity.AI.Navigation;

namespace Assets.DungeonGenerator
{
    [RequireComponent(typeof(DungeonComponents))]
    public class DungeonGenerator : MonoBehaviour
    {
        private DungeonComponents components;
        private readonly IDungeonAlgorithm grammarDungeonGenerator;

        private void Start()
        {
            // grammarDungeonGenerator = new MissionGrammarAlgorithm();
            components = GetComponent<DungeonComponents>();
        }

        /**
        *  Generates a new dungeon using the provided dungeon parameters.
        */
        public void GenerateDungeon(Dungeon dungeon)
        {
            // TODO: Run dungeon parameters through misson grammar, then pass result to BSP algorithm
            // TODO: Might need to build navmesh before placing enemies
            IDungeonAlgorithm algorithm = new BSPAlgorithm();
            algorithm.GenerateRepresentation(dungeon);
            algorithm.ConstructDungeon(components);
            algorithm.PlaceContent(components);
            GetComponent<NavMeshSurface>().BuildNavMesh();
        }
    }
}