using UnityEngine;

namespace Assets.DungeonGenerator.Components
{
    /**
     * TODO
     */
    public struct Dungeon
    {
        public Vector2 Size { get; set; }
        public Vector2 MaxRoomSize { get; private set; }
        public Vector2 MinRoomSize { get; private set; }
        public Vector2 MinCorridorSize { get; internal set; }
        public Vector3 playerStartPos;

        public float rootDungeonSplit;
        public float enemySpawnRate;
        public float itemSpawnRate;
        internal int minEnemiesPerRoom;
        internal int maxEnemiesPerRoom;
        internal int maxItemsPerRoom;
        internal int minItemsPerRoom;

        /**
         * TODO:
         */
        public Dungeon(Vector2 size, Vector2 minRoomSize, Vector2 maxRoomSize, Vector2 minCorridorSize, float enemySpawnRate, float itemSpawnRate, float rootDungeonSplit, int minItemsPerRoom, int maxItemsPerRoom, int minEnemiesPerRoom, int maxEnemiesPerRoom) : this()
        {
            Size = size;
            MaxRoomSize = maxRoomSize;
            MinRoomSize = minRoomSize;
            MinCorridorSize = minCorridorSize;
            this.enemySpawnRate = enemySpawnRate;
            this.itemSpawnRate = itemSpawnRate;
            this.minItemsPerRoom = minItemsPerRoom;
            this.maxItemsPerRoom = maxItemsPerRoom;
            this.rootDungeonSplit = rootDungeonSplit;
            this.minEnemiesPerRoom = minEnemiesPerRoom;
            this.maxEnemiesPerRoom = maxEnemiesPerRoom;
        }
    }
}