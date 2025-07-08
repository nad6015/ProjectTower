using UnityEngine;
using Assets.DungeonGenerator.Components;
using Assets.CombatSystem;
using Unity.Mathematics;

namespace Assets.DungeonGenerator
{
    using Random = UnityEngine.Random;

    public class DungeonMaster : MonoBehaviour
    {
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

        private void Start()
        {
            //Random.InitState(1); // TODO: Seed should be randomised between sessions. Set to 1 for dev
            dungeonGenerator = GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<DungeonGenerator>();
        }

        public void OnNewDungeon()
        {
            dungeonGenerator.GenerateDungeon(CreateDungeon());
        }

        Dungeon CreateDungeon()
        {
            return new Dungeon(RandomDungeonSize(), MinRoomSize, MaxRoomSize, MinCorridorSize,
                enemySpawnRate, itemSpawnRate, rootDungeonSplit, minItemsPerRoom, maxItemsPerRoom,
                minEnemiesPerRoom, maxEnemiesPerRoom);
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
            int health = 3;//playerCharacter.GetStat(FighterStats.HEALTH);
            if (health > 4)
            {
                minItemsPerRoom = 0;
                maxItemsPerRoom = 2;
                enemySpawnRate = 1;
                return DungeonSize.LARGE;
            }
            else if (health < 2)
            {
                minItemsPerRoom = 3;
                maxItemsPerRoom = 5;
                enemySpawnRate = 0;
                itemSpawnRate = 1;
                return DungeonSize.SMALL;
            }

            return DungeonSize.MEDIUM;
        }

        enum DungeonSize
        {
            SMALL = 0,
            MEDIUM = 1,
            LARGE = 2,
        }
    }
}