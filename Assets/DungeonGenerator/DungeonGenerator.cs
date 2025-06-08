using UnityEngine;
using Assets.DungeonGenerator.Components;
using Unity.Mathematics;

namespace Assets.DungeonGenerator {
    using Random = UnityEngine.Random;

    public class DungeonGenerator : MonoBehaviour
    {
        public Vector2 MaxDungeonSize;
        public Vector2 MinDungeonSize;

        public Vector2 MaxRoomSize;
        public Vector2 MinRoomSize;

        private DungeonConstructor dungeonConstructor;
        private readonly IDungeonAlgorithm grammarDungeonGenerator;
        private Dungeon dungeon;

        private void Start()
        {
            // grammarDungeonGenerator = new MissionGrammarAlgorithm();
            dungeonConstructor = GetComponent<DungeonConstructor>();
            
            Random.InitState(1); // TODO: Seed should be randomised between sessions. Set to 1 for dev
        }

        /**
      *  Generates a new dungeon using the provided input.
      *  TODO: Input as parameters
      */
        public void GenerateDungeon()
        {
            GenerateDungeon(new BSPAlgorithm());
        }

        /**
         *  Generates a new dungeon using the provided input.
         *  TODO: Input as parameters
         */
        private void GenerateDungeon(IDungeonAlgorithm algorithm)
        {
            InitialiseDungeonSize();
            algorithm.GenerateRepresentation(dungeon);
            algorithm.ForEachRoom(room => room.ConstructRoom(dungeonConstructor.floorAsset, dungeonConstructor.wallAsset));
        }

        /*
         * Step 1: Generator dungeon representation
         * Step 2: Construct dungeon based on representation
         * 
         * Generate room representation:
         *  1. Choose a random size within the min and max bounds (note: this could be a parameter)
         *  2. Choose a random axis (verticle or horizontal) and divide along it (coords can be random)
         *  3. Choose one of the two paritions then repeat steps 2 and 3 until min room bound is met.
         *  4. Repeat for the other partition.
         * 
         * Generator corridor representation:
         *  1. Starting from the bottom-left most parent node, choose a random axis and link the children nodes via a straight line.
         *  2. Repeat for other parent nodes at the same level.
         *  3. Repeat steps 1 and 2 until the root node is reached.
         *  
         * What I'll need:
         *  - A class/struct for room representation. It should contain coordinates, size and the linking corridor.
         *  - A class/struct for corridors. It should contain coordinates and size.
         *  - A tree structure. What does C# provide natively?
         *  - A class for dungeon representation. It should contain coordinates, size, theme and the BSP tree.
         *  
         * Construct dungeon:
         *  1. Take the Dungeon representation and generate a plane the size of the dungeon. This will be the floor.
         *  2. Take the BSP tree and, starting from the bottom-leftmost node, create the room by placing wall tiles along the boundaries,
         *      leaving a gap for where the corridor should be placed.
         *  3. From the gap for the corridor, place wall tiles to construct the corridor.
         *  4. Repeat steps 2 and 3 until all the rooms have been built and linked via corridors.
         *  
         * What I'll need:
         *  - A MonoBehaviour Script that places assets.
         *  - A DungeonGenerator Script that calls various functions.
         *  - Wall tiles (can be a prototype can this stage)
         *  
         * What I haven't considered yet, but need to:
         *  - Content. How and where am I placing things?
         *  - Where the entrance is. The player must start somewhere on the edge of the dungeon (for now. This will once the dungeon layouts starts getting     more interesting)
         *  - When do I think about making the AI agent for automated testing?
         *  - Need to make the dungeons completable (i.e. have an exit)
         *  
         * How to test/evaluate:
         *  - Unit tests for the dungeon representation generator (so will need to visualise/represent the data in a testable form)
         *  - Too early for profiling? Will try regardless.
         *  - Create basic dungeon for player to explore (doesn't have to be completable yet)
         */

        void InitialiseDungeonSize()
        {
            float width = math.floor(Random.Range(MinDungeonSize.x, MaxDungeonSize.x));
            float height = math.floor(Random.Range(MinDungeonSize.y, MaxDungeonSize.y));

            dungeon = new Dungeon(new Vector2(width, height), MinRoomSize, MaxRoomSize);
        }
    }
}