using UnityEngine;
using Assets.DungeonGenerator.Components;
using Assets.PlayerCharacter;
using Unity.Mathematics;
using Assets.GameManager;
using System.Data.Common;

namespace Assets.DungeonGenerator
{
    using Random = UnityEngine.Random;

    public partial class DungeonMaster : MonoBehaviour
    {
        public DungeonMasterRuleset Ruleset { get; set; }
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
        private DungeonParameters _dungeonParams;
        private float _avgTimeBetweenEnemyDefeats = 0;
        private float _enemiesDefeated = 0;
        private GameSceneManager _sceneManager;

        public void Start()
        {
            _dungeonGenerator = GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<DungeonGenerator>();
            _sceneManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameSceneManager>();
            //_dungeonParams = LoadDungeonParameters();
            State = DungeonMasterState.GENERATE_DUNGEON;
            //Random.InitState(1); // TODO: Seed should be randomised between sessions. Set to 1 for dev
        }

        public void OnDungeonCleared()
        {
            State = DungeonMasterState.GENERATE_DUNGEON;
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
                case DungeonMasterState.GENERATE_DUNGEON:
                {
                    GenerateDungeon();
                    break;
                }
                case DungeonMasterState.RUNNING:
                {
                    MonitorGameplay();
                    break;
                }
                case DungeonMasterState.GAME_END:
                {
                    EndGame();
                    break;
                }
            }
        }

        private void EndGame()
        {
            throw new System.NotImplementedException();
            // _sceneManager.SceneTransition(GameSceneManager.GameScene.GAME_WON);
        }

        private void MonitorGameplay()
        {
            Ruleset.ForEach(rule =>
            {
                //if (rule.AreConditionsMet(_dungeonParams[rule.Parameter]))
                //{
                //    _dungeo
                //    ModifyNextDungeonParams(rule.ReturnValue());
                //}
                //else
                //{
                //    ModifyEventChance(rule.ReturnValue());
                //}
            });
        }

        private void GenerateDungeon()
        {
            NewDungeon();

            _currentDungeon.DungeonExit.DungeonCleared += OnDungeonCleared;
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            State = DungeonMasterState.RUNNING;
            Floor++;
        }

        private void NewDungeon()
        {
            _dungeonGenerator.ClearDungeon();
            _currentDungeon = _dungeonGenerator.GenerateDungeon(GenerateDungeonParameters());
            GameObject startingPoint = GameObject.FindGameObjectWithTag("PlayerSpawn");

            if (_player != null)
            {
                _currentDungeon.StartingPoint.Spawn(_player.transform);
            }
            else
            {
                startingPoint.GetComponent<SpawnPoint>().Spawn();
            }
        }
    }
}