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
using static Assets.DungeonMaster.DungeonMasterDeserializationUtil;
using Assets.Audio;

namespace Assets.DungeonMaster
{
    public class DungeonMaster : MonoBehaviour
    {
        [SerializeField]
        private List<DungeonComponents> _componentsList;

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
        private DungeonRepresentation _dungeonParams;
        private DungeonRepresentation _nextDungeonParams;
        private Dictionary<GameplayParameter, ValueRepresentation> _gameParams;
        private Dictionary<GameParameter, int> _floorStatistics;
        //private ResourceSystem _resourceSystem;
        private CombatManager _combatSystem;
        private SceneTransitionManager _sceneTransitionManager;
        private AudioManager _audioManager;
        private DungeonMasterConfiguration _config;
        private int currentSection = 0;
        private DungeonComponents _currentComponents;
        private Dictionary<DungeonParameter, ValueRepresentation> _dungeonParameters;

        public void Start()
        {
            _floorStatistics = new();
            _dungeonGenerator = FindComponentByTag<DungeonGenerator.DungeonGenerator>("DungeonGenerator");
            _combatSystem = FindComponentByTag<CombatManager>("CombatSystem");
            _sceneTransitionManager = FindComponentByTag<SceneTransitionManager>("SceneManager");
            _audioManager = FindComponentByTag<AudioManager>("AudioManager");
            CurrentFloor = 0;
            Debug.Log(CurrentFloor);
            NextDungeonSection();

            ReadConfigurationFromFiles();

            _combatSystem.EnemyDefeated += OnEnemyDefeated;
            _combatSystem.PlayerDefeated += OnPlayerDefeated;

            if (_randomSeed == -1)
            {
                _randomSeed = Random.Range(0, int.MaxValue);
            }
            Random.InitState(_randomSeed);
            Debug.Log("Random seed is " + _randomSeed);
            State = DungeonMasterState.GenerateDungeon;
        }

        public void OnDungeonCleared()
        {
            _player.Pause();
            _dungeonGenerator.ClearDungeon();
            State = CurrentFloor > MaxFloors ? DungeonMasterState.GameEnd : DungeonMasterState.GenerateDungeon;
        }

        private void FixedUpdate()
        {
            switch (State)
            {
                case DungeonMasterState.GenerateDungeon:
                {
                    GenerateDungeon();
                    break;
                }
                case DungeonMasterState.Running:
                {
                    DoWork();
                    break;
                }
                case DungeonMasterState.GameEnd:
                {
                    EndGame();
                    break;
                }
            }
        }

        /// <summary>
        /// Generates the next dungeon using the adapted dungeon parameters.
        /// </summary>
        private void GenerateDungeon()
        {
            if (CurrentFloor > FloorsPerSection)
            {
                NextDungeonSection();
            }

            DungeonMission nextMission = (DungeonMission)CurrentFloor;

            _dungeonParams = new DungeonRepresentation(_config.BaseDungeons[nextMission],
                _config.DungeonFlows[nextMission], _currentComponents, _dungeonParameters);

            NewDungeon();

            FindComponentByTag<DungeonExit>("DungeonExit").DungeonCleared += OnDungeonCleared;

            _sceneTransitionManager.SceneTransition(GameScene.None);
            _audioManager.Modify(_dungeonParams.Components);
            _audioManager.PlayBackgroundMusic();

            _combatSystem.OnNewDungeon();

            _player.Resume();
            _player.Camera.UpdateBackgroundColor(_currentComponents.tilemap.mainCameraColor);
            CurrentFloor++; // Next floor has been reached, so increment counter

            State = DungeonMasterState.Running;
        }

        /// <summary>
        /// Main loop of the Dungeon Master. Checks all the rules for both dungeon generation and gameplay
        /// and updates their values if the rules' conditions are met.
        /// </summary>
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

        /// <summary>
        /// Transitions the game to the next scene based on whether the player won or lost.
        /// </summary>
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

        /// <summary>
        /// Calls the dungeon generator and sets the player in the new starting room.
        /// </summary>
        private void NewDungeon()
        {
            _dungeonGenerator.GenerateDungeon(_dungeonParams);
            SpawnPoint startingPoint = FindComponentByTag<SpawnPoint>("PlayerSpawn");

            if (_player != null)
            {
                startingPoint.Spawn(_player.transform);
            }
            else
            {
                _player = startingPoint.Spawn().GetComponent<PlayerController>();
            }
        }

        /// <summary>
        /// Reads config from the currently configuration json file.
        /// </summary>
        private void ReadConfigurationFromFiles()
        {
            _config = ReadGeneratorConfigFromJson(_dungeonFlowsFile);

            JObject json = JObject.Parse(_defaultRulesetFile.text);
            GenerationRuleset = BuildDungeonRuleset(json);
            GameplayRuleset = BuildGameplayParams(json);

            _dungeonParameters = BuildDungeonParameters(_defaultParamFile);
        }

        private void NextDungeonSection()
        {
            _currentComponents = _componentsList[currentSection++];
            CurrentFloor = 0;

        }
        private void OnEnemyDefeated(NpcFighter fighter)
        {
            if (_floorStatistics.ContainsKey(GameParameter.EnemiesDefeated))
            {
                _floorStatistics[GameParameter.EnemiesDefeated]++;
            }
            else
            {
                _floorStatistics[GameParameter.EnemiesDefeated] = 1;
            }
        }

        private void OnPlayerDefeated(PlayableFighter fighter)
        {
            State = DungeonMasterState.GameEnd;
        }
    }
}