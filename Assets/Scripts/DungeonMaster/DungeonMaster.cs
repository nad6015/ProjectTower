using UnityEngine;
using System.Collections.Generic;
using Assets.DungeonGenerator.Components;
using Assets.PlayerCharacter;
using Assets.GameManager;
using Assets.Combat;
using Newtonsoft.Json.Linq;
using Assets.DungeonGenerator;

namespace Assets.DungeonMaster
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

        private DungeonGenerator.DungeonGenerator _dungeonGenerator;
        private GameObject _player;
        private Dungeon _currentDungeon;
        private DungeonRepresentation _dungeonParams;
        private DungeonRepresentation _nextDungeonParams;
        private Dictionary<GameplayParameter, ValueRepresentation> _gameParams;
        private Dictionary<GameParameter, int> _floorStatistics;
        //private ResourceSystem _resourceSystem;
        private CombatSystem _combatSystem;

        public void Start()
        {
            _floorStatistics = new();
            _dungeonGenerator = GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<DungeonGenerator.DungeonGenerator>();
            _combatSystem = GameObject.FindGameObjectWithTag("CombatSystem").GetComponent<CombatSystem>();

            JObject json = JObject.Parse(FindFile("DefaultDungeonRuleset", _rulesets).text);
            GenerationRuleset = RulesetBuilder.BuildDungeonRuleset(json);
            GameplayRuleset = RulesetBuilder.BuildGameplayParams(json);

            _dungeonParams = new DungeonRepresentation(FindFile("DefaultDungeonParameters", _parameterFiles));

            _combatSystem.EnemyDefeated += OnEnemyDefeated;
            _combatSystem.PlayerDefeated += OnPlayerDefeated;

            Random.InitState(_randomSeed);
            State = DungeonMasterState.GENERATE_DUNGEON;
        }

        public void OnDungeonCleared()
        {
            _dungeonParams = _nextDungeonParams;
            _nextDungeonParams = null;
            _player.SetActive(false);
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
            _player.SetActive(true);
            State = DungeonMasterState.RUNNING;
            Floor++;
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
                _player = startingPoint.Spawn();
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