using UnityEngine;
using Assets.DungeonGenerator.Components;
using Assets.PlayerCharacter;
using Unity.Mathematics;
using Assets.GameManager;

namespace Assets.DungeonGenerator
{
    using Random = UnityEngine.Random;

    public class DungeonMaster : MonoBehaviour
    {
        public DungeonMasterState State { get; private set; }
        public int Floor { get; private set; }

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

        private DungeonGenerator _dungeonGenerator;
        private Vector2 maxDungeonSize;
        private PlayerController _player;
        private Dungeon _currentDungeon;
        private float _avgTimeBetweenEnemyDefeats = 0;
        private float _enemiesDefeated = 0;
        private GameSceneManager _sceneManager;

        private void Awake()
        {
            State = DungeonMasterState.AWAITING_START;
            //Random.InitState(1); // TODO: Seed should be randomised between sessions. Set to 1 for dev
            _dungeonGenerator = GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<DungeonGenerator>();
            _sceneManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameSceneManager>();
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

        private void Update()
        {
            switch (State)
            {
                case DungeonMasterState.AWAITING_START:
                break; //no-op
                case DungeonMasterState.ENTER_DUNGEON:
                {
                    GenerateDungeon();
                    break;
                }
                case DungeonMasterState.SET_MONITORING_TARGETS:
                {
                    SetMonitoringTargets();
                    break;
                }
                case DungeonMasterState.RUNNING:
                break;
                case DungeonMasterState.PAUSED:
                break;
                case DungeonMasterState.DUNGEON_CLEARED:
                {
                    OnDungeonClear();
                    break;
                }
                case DungeonMasterState.TOWER_BEATEN:
                {
                    _sceneManager.SceneTransition(GameSceneManager.GameScene.GAME_WON);
                    break;
                }
            }
        }

        private void OnDungeonClear()
        {
            _dungeonGenerator.ClearDungeon();
            _player.enabled = false;
            // TODO: Modify parameters based on game data
            State = DungeonMasterState.ENTER_DUNGEON;
            Floor++;
        }

        private void SetMonitoringTargets()
        {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            State = DungeonMasterState.RUNNING;
        }

        private void GenerateDungeon()
        {
            NewDungeon();
            State = DungeonMasterState.SET_MONITORING_TARGETS;
        }

        public void StartDungeonMaster()
        {
            State = DungeonMasterState.ENTER_DUNGEON;
        }

        public bool IsReady()
        {
            return State == DungeonMasterState.RUNNING;
        }

        public void Pause()
        {
            State = DungeonMasterState.PAUSED;
        }

        public void DungeonCleared()
        {
            State = DungeonMasterState.DUNGEON_CLEARED;
        }

        public void NewDungeon()
        {
            _currentDungeon = _dungeonGenerator.GenerateDungeon(GenerateDungeonParameters());
            GameObject startingPoint = GameObject.FindGameObjectWithTag("PlayerSpawn");

            if (_player != null)
            {
                _player.transform.SetPositionAndRotation(startingPoint.transform.position, Quaternion.identity);
                _player.enabled = true;
            }
            else
            {
                startingPoint.GetComponent<SpawnPoint>().Spawn();
            }
        }

        public enum DungeonMasterState
        {
            AWAITING_START,
            ENTER_DUNGEON,
            PAUSED,
            SET_MONITORING_TARGETS,
            RUNNING,
            DUNGEON_CLEARED,
            THRESHOLD_REACHED,
            TOWER_BEATEN
        }
    }
}