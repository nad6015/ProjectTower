using UnityEngine;
using System.Collections.Generic;
using Assets.DungeonGenerator.Components;
using Assets.PlayerCharacter;
using Assets.GameManager;
using Assets.Combat;
using Newtonsoft.Json.Linq;
using Assets.DungeonGenerator;
using Random = UnityEngine.Random;
using static Assets.Utilities.GameObjectUtilities;
using Assets.Audio;

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

        [SerializeField]
        private TextAsset _dungeonFlowsFile;

        [field: SerializeField]
        public int MaxFloors { get; private set; }

        [field: SerializeField]
        public int FloorsPerSection { get; private set; }
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
        private AudioManager _audioManager;
        private DungeonMasterConfiguration _config;


        public void Start()
        {
            _floorStatistics = new();
            _dungeonGenerator = FindComponentByTag<DungeonGenerator.DungeonGenerator>("DungeonGenerator");
            _combatSystem = FindComponentByTag<CombatSystem>("CombatSystem");
            _sceneTransitionManager = FindComponentByTag<SceneTransitionManager>("SceneManager");
            _audioManager = FindComponentByTag<AudioManager>("AudioManager");
            ReadConfig();

            _combatSystem.EnemyDefeated += OnEnemyDefeated;
            _combatSystem.PlayerDefeated += OnPlayerDefeated;

            if (_randomSeed > -1)
            {
                Random.InitState(_randomSeed);
            }
            State = DungeonMasterState.GENERATE_DUNGEON;
        }

        public void OnDungeonCleared()
        {
            //_dungeonParams = _nextDungeonParams;
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

            switch (CurrentFloor % FloorsPerSection)
            {
                case 0: // Current floor is a multiple of three
                {
                    break; // TODO
                }
                case 1: // Current floor is a multiple of two
                {
                    _dungeonParams.LoadFlows(_config.DungeonFlows[DungeonMission.UnlockDoor]);
                    break;
                }
                case 2:
                {
                    _dungeonParams.LoadFlows(_config.DungeonFlows[DungeonMission.ExploreFloor]);
                    break;
                }
                default:
                {
                    Debug.Log("CurrentFloor mod FloorsPerSection = " + CurrentFloor % FloorsPerSection);
                    break;
                }
            }

            NewDungeon();
            FindComponentByTag<DungeonExit>("DungeonExit").DungeonCleared += OnDungeonCleared;
            _sceneTransitionManager.SceneTransition(GameScene.None);
            _audioManager.Modify(_dungeonParams.Components);
            _audioManager.PlayBackgroundMusic();
            _player.Play();
            State = DungeonMasterState.RUNNING;
            CurrentFloor++; // Next floor has been reached, so increment counter
        }

        private void NewDungeon()
        {
            _dungeonGenerator.ClearDungeon();
            _currentDungeon = _dungeonGenerator.GenerateDungeon(_dungeonParams);
            SpawnPoint startingPoint = FindComponentByTag<SpawnPoint>("PlayerSpawn");

            //_nextDungeonParams = _dungeonParams;

            if (_player != null)
            {
                startingPoint.Spawn(_player.transform);
            }
            else
            {
                _player = startingPoint.Spawn().GetComponent<PlayerController>();
            }
        }

        private void ReadConfig()
        {
            _config = new()
            {
                DungeonFlows = new()
            };

            JObject jFlows = JObject.Parse(_dungeonFlowsFile.text);
            JsonUtils.ForEachIn(jFlows, dungeonFlow =>
            {
                List<FlowPattern> flowPatterns = new();

                JsonUtils.ForEachIn(jFlows[dungeonFlow.Path], pattern =>
                {
                    flowPatterns.Add(new FlowPattern(pattern["matches"], pattern["replacer"]));
                });

                _config.DungeonFlows.Add(JsonUtils.ConvertToEnum<DungeonMission>(dungeonFlow.Path), flowPatterns);
            });
            JObject json = JObject.Parse(_defaultRulesetFile.text);
            GenerationRuleset = RulesetBuilder.BuildDungeonRuleset(json);
            GameplayRuleset = RulesetBuilder.BuildGameplayParams(json);

            _dungeonParams = new DungeonRepresentation(_defaultParamFile);
        }


        private TextAsset FindFile(string v, List<TextAsset> files)
        {
            return files.Find(t => t.name.Contains(v));
        }

        private void OnEnemyDefeated(Fighter fighter)
        {
            if (fighter.GetComponent<NpcFighter>() != null)
            {
                _floorStatistics[GameParameter.EnemiesDefeated]++;
            }
        }

        private void OnPlayerDefeated(Fighter fighter)
        {
            State = DungeonMasterState.GAME_END;
        }
    }
}