using UnityEngine;
using Assets.DungeonGenerator.Components;
using Assets.PlayerCharacter;
using Assets.GameManager;
using System.Collections.Generic;

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

        public DungeonMasterRuleset Ruleset { get; private set; }
        public DungeonMasterState State { get; private set; }

        public int Floor { get; private set; }

        private DungeonGenerator _dungeonGenerator;
        private PlayerController _player;
        private Dungeon _currentDungeon;
        private DungeonParameters _dungeonParams;
        private DungeonParameters _nextDungeonParams;
        private Dictionary<string, float> _gameData;

        public void Start()
        {
            _dungeonGenerator = GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<DungeonGenerator>();

            Ruleset = new DungeonMasterRuleset("TestRuleset");
            _dungeonParams = new DungeonParameters(FindByName("DefaultDungeonParameters").name);
            _gameData = new();

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
            Ruleset.ForEach(rule =>
            {
                if (_gameData.ContainsKey(rule.Id) && rule.ConditionsMet(_gameData[rule.Id]))
                {
                    ModifyNextDungeonParams(rule.RuleValue());
                }
            });
        }

        private void ModifyNextDungeonParams(RuleValue ruleValue)
        {
            // TODO
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
            return _parameterFiles.Find(t =>  t.name.Contains(v));
        }
    }
}