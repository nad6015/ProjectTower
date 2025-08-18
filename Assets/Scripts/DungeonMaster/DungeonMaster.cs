using UnityEngine;
using System.Collections.Generic;
using Assets.DungeonGenerator.Components;
using Assets.PlayerCharacter;
using Assets.GameManager;
using Assets.Combat;
using Newtonsoft.Json.Linq;
using Assets.DungeonGenerator;
using Random = UnityEngine.Random;

namespace Assets.DungeonMaster
{
    public partial class DungeonMaster : MonoBehaviour
    {
        [SerializeField]
        private List<TextAsset> _parameterFiles;

        [SerializeField]
        private List<TextAsset> _rulesets;

        [SerializeField]
        private int _randomSeed;

        [SerializeField]
        private TextAsset _defaultParamFile;

        [SerializeField]
        private TextAsset _defaultRulesetFile;

        [field: SerializeField]
        public int MaxFloors { get; private set; }
        public int CurrentFloor { get; private set; }

        public DungeonMasterState State { get; private set; }

        private Dictionary<DungeonParameter, DungeonRule> GenerationRuleset { get; set; }
        private Dictionary<GameplayParameter, GameplayRule> GameplayRuleset { get; set; }

        private DungeonGenerator.DungeonGenerator _dungeonGenerator;
        private PlayerController _player;
        private Dungeon _currentDungeon;
        private DungeonRepresentation _dungeonParams;
        private DungeonRepresentation _nextDungeonParams;
        private Dictionary<GameplayParameter, ValueRepresentation> _gameParams;
        private Dictionary<GameParameter, int> _floorStatistics;
        //private ResourceSystem _resourceSystem;
        private CombatSystem _combatSystem;
        private SceneTransitionManager _sceneTransitionManager;

        public void Start()
        {
            _floorStatistics = new();
            _dungeonGenerator = GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<DungeonGenerator.DungeonGenerator>();
            _combatSystem = GameObject.FindGameObjectWithTag("CombatSystem").GetComponent<CombatSystem>();
            _sceneTransitionManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneTransitionManager>();

            JObject json = JObject.Parse(_defaultRulesetFile.text);
            GenerationRuleset = RulesetBuilder.BuildDungeonRuleset(json);
            GameplayRuleset = RulesetBuilder.BuildGameplayParams(json);

            _dungeonParams = new DungeonRepresentation(_defaultParamFile);

            _combatSystem.EnemyDefeated += OnEnemyDefeated;
            _combatSystem.PlayerDefeated += OnPlayerDefeated;

            Random.InitState(_randomSeed);
            State = DungeonMasterState.GENERATE_DUNGEON;
        }

        public void OnDungeonCleared()
        {
            _dungeonParams = _nextDungeonParams;
            //_nextDungeonParams = null;
            _player.Pause();
            if (CurrentFloor >= MaxFloors)
            {
                State = DungeonMasterState.GAME_END;
            }
            else
            {
                State = DungeonMasterState.GENERATE_DUNGEON;
            }
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
                _sceneTransitionManager.SceneTransition(GameScene.GameLost);
            }
            else
            {
                _sceneTransitionManager.SceneTransition(GameScene.NextScene);
            }
        }

        private void DoWork()
        {
            foreach (DungeonRule rule in GenerationRuleset.Values)
            {
                if (rule.ConditionsMet(_floorStatistics))
                {
                    _nextDungeonParams.ModifyParameter(rule.Parameter, rule.Value());
                }
            }

            foreach (GameplayRule rule in GameplayRuleset.Values)
            {
                if (rule.ConditionsMet(_floorStatistics))
                {
                    _gameParams[rule.Parameter].Modify(rule.Value());
                }
            }
        }

        private void GenerateDungeon()
        {
            NewDungeon();
            GameObject.FindGameObjectWithTag("DungeonExit").GetComponent<DungeonExit>().DungeonCleared += OnDungeonCleared;
            _sceneTransitionManager.SceneTransition(GameScene.None);
            _player.Play();
            State = DungeonMasterState.RUNNING;
            CurrentFloor++;
        }

        private void NewDungeon()
        {
            _dungeonGenerator.ClearDungeon();
            _currentDungeon = _dungeonGenerator.GenerateDungeon(_dungeonParams);
            SpawnPoint startingPoint = GameObject.FindGameObjectWithTag("PlayerSpawn").GetComponent<SpawnPoint>();

            _nextDungeonParams = _dungeonParams;

            if (_player != null)
            {
                startingPoint.Spawn(_player.transform);
            }
            else
            {
                _player = startingPoint.Spawn().GetComponent<PlayerController>();
            }
        }

        private TextAsset FindFile(string v, List<TextAsset> files)
        {
            return files.Find(t => t.name.Contains(v));
        }

        private void OnEnemyDefeated(NpcFighter fighter)
        {
            _floorStatistics[GameParameter.EnemiesDefeated]++;
        }

        private void OnPlayerDefeated(Fighter fighter)
        {
            State = DungeonMasterState.GAME_END;
        }
    }
}