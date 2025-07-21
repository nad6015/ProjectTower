using UnityEngine;
using Assets.DungeonGenerator.Components;
using Assets.PlayerCharacter;
using Unity.Mathematics;

namespace Assets.DungeonGenerator
{
    using Random = UnityEngine.Random;

    public class DungeonMaster : MonoBehaviour
    {
        public State state { get; private set; }
        public Vector2 LargeDungeonSize;
        public Vector2 MediumDungeonSize;
        public Vector2 SmallDungeonSize;
        public Vector2 MinDungeonSize;

        public Vector2 MaxRoomSize;
        public Vector2 MinRoomSize;

        public Vector2 MinCorridorSize;

        public float rootDungeonSplit = 0.5f;

        public float enemySpawnRate = 0.5f;

        public int minEnemiesPerRoom;
        public int maxEnemiesPerRoom;

        public float itemSpawnRate = 0.5f;

        public int minItemsPerRoom;
        public int maxItemsPerRoom;

        private DungeonGenerator dungeonGenerator;
        private Vector2 maxDungeonSize;
        private PlayerController _player;
        private Dungeon _currentDungeon;
        private float _avgTimeBetweenEnemyDefeats = 0;
        private float _enemiesDefeated = 0;

        private void Awake()
        {
            state = State.AWAITING_START;
            //Random.InitState(1); // TODO: Seed should be randomised between sessions. Set to 1 for dev
            dungeonGenerator = GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<DungeonGenerator>();
        }

        public void OnNewDungeon()
        {
            _currentDungeon = dungeonGenerator.GenerateDungeon(GenerateDungeonParameters());
        }

        DungeonParameters GenerateDungeonParameters()
        {
            // TODO: Run FSM on game state
            return new DungeonParameters(RandomDungeonSize(), MinRoomSize, MaxRoomSize, MinCorridorSize,
                enemySpawnRate, itemSpawnRate, rootDungeonSplit, minItemsPerRoom, maxItemsPerRoom,
                minEnemiesPerRoom, maxEnemiesPerRoom, 10);
        }

        Vector2 RandomDungeonSize()
        {
            switch (DetermineDungeonSize())
            {
                case DungeonSize.SMALL:
                    maxDungeonSize = SmallDungeonSize;
                    break;
                case DungeonSize.MEDIUM:
                    maxDungeonSize = MediumDungeonSize;
                    break;
                case DungeonSize.LARGE:
                    maxDungeonSize = LargeDungeonSize;
                    break;
            }
            float width = math.floor(Random.Range(MinDungeonSize.x, maxDungeonSize.x));
            float height = math.floor(Random.Range(MinDungeonSize.y, maxDungeonSize.y));

            return new Vector2(width, height);
        }

        DungeonSize DetermineDungeonSize()
        {
            return DungeonSize.LARGE;
        }

        enum DungeonSize
        {
            SMALL = 0,
            MEDIUM = 1,
            LARGE = 2,
        }

        public enum State
        {
            AWAITING_START,
            STARTING,
            PAUSED,
            MONITORING,
            DUNGEON_CLEARED,
            THRESHOLD_REACHED
        }

        private void Update()
        {
            switch (state)
            {
                case State.AWAITING_START: break; //no-op
                case State.STARTING:
                    {
                        Initialise();
                        break;
                    }
                case State.PAUSED: break;
            }
        }

        private void Initialise()
        {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            state = State.MONITORING;
        }

        public void StartDungeonMaster()
        {
            state = State.STARTING;
        }

        public bool IsReady()
        {
            return state != State.AWAITING_START && state != State.STARTING;
        }

        public void Pause()
        {
            state = State.PAUSED;
        }
    }
}