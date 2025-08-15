using UnityEngine;
using System.Collections.Generic;
using Assets.DungeonGenerator.Components;
using Assets.PlayerCharacter;
using Assets.GameManager;
using Assets.Combat;
using Newtonsoft.Json.Linq;

namespace Assets.DungeonGenerator
{
    using Random = UnityEngine.Random;

    public partial class DungeonMaster : MonoBehaviour
    {
        [SerializeField]
        private List<TextAsset> _parameterFiles;

        [SerializeField]
        private List<TextAsset> _rulesets;

        [SerializeField]
        private int _randomSeed;

        public Dictionary<DungeonParameter, DungeonRule> GenerationRuleset { get; private set; }
        public Dictionary<GameplayParameter, GameplayRule> GameplayRuleset { get; private set; }
        public DungeonMasterState State { get; private set; }

        public int Floor { get; private set; }

        private DungeonGenerator _dungeonGenerator;
        private PlayerController _player;
        private Dungeon _currentDungeon;
        private DungeonRepresentation _dungeonParams;
        private DungeonRepresentation _nextDungeonParams;
        private Dictionary<GameplayParameter, ValueRepresentation> _gameParams;
        private Dictionary<GameParameter, int> _gameData;
        //private ResourceSystem _resourceSystem;
        private CombatSystem _combatSystem;
        private GameStatistics _floorStatistics;

        public void Start()
        {
            _dungeonGenerator = GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<DungeonGenerator>();
            _combatSystem = GameObject.FindGameObjectWithTag("CombatSystem").GetComponent<CombatSystem>();
            _gameData = new();

            JObject json = JObject.Parse(FindByName("DefaultDungeonRuleset").text);
            GenerationRuleset = RulesetBuilder.BuildDungeonRuleset(json);
            GameplayRuleset = RulesetBuilder.BuildGameplayParams(json);

            _dungeonParams = new DungeonRepresentation(FindByName("DefaultDungeonParameters"));

            _combatSystem.EnemyDefeated += OnEnemyDefeated;
            _combatSystem.PlayerDefeated += OnPlayerDefeated;

            Random.InitState(_randomSeed);
            State = DungeonMasterState.GENERATE_DUNGEON;
        }


        public void OnDungeonCleared()
        {
            _dungeonParams = _nextDungeonParams;
            _nextDungeonParams = null;
            State = DungeonMasterState.GENERATE_DUNGEON;
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
                    DoWork();

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
            if (_player.GetComponent<PlayableFighter>().IsDead())
            {
                GameSceneManager.SceneTransition(GameSceneManager.GameScene.GAME_LOST);
            }
        }

        private void DoWork()
        {
            foreach (DungeonRule rule in GenerationRuleset.Values)
            {
                if (rule.ConditionsMet(_gameData))
                {
                    _nextDungeonParams.ModifyParameter(rule.Parameter, rule.Value());
                }
            }

            foreach (GameplayRule rule in GameplayRuleset.Values)
            {
                if (rule.ConditionsMet(_gameData))
                {
                    _gameParams[rule.Parameter].Modify(rule.Value());
                }
            }
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
            _currentDungeon = _dungeonGenerator.GenerateDungeon(_dungeonParams);
            GameObject startingPoint = GameObject.FindGameObjectWithTag("PlayerSpawn");

            _nextDungeonParams = _dungeonParams;

            if (_player != null)
            {
                _currentDungeon.StartingPoint.Spawn(_player.transform);
            }
            else
            {
                startingPoint.GetComponent<SpawnPoint>().Spawn();
            }
        }

        private TextAsset FindByName(string v)
        {
            return _parameterFiles.Find(t => t.name.Contains(v));
        }

        private void OnEnemyDefeated(NpcFighter fighter)
        {
            _floorStatistics.enemiesDefeated++;
        }

        private void OnPlayerDefeated(Fighter fighter)
        {
            State = DungeonMasterState.GAME_END;
        }
    }

    public struct GameStatistics
    {
        public int enemiesDefeated;
    }
}