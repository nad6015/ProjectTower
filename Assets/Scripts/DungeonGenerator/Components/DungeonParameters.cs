using UnityEngine;

namespace Assets.DungeonGenerator.Components
{
    /// <summary>
    /// Contains the parameters needed to generate a dungeon 
    /// </summary>
    public struct DungeonParameters
    {
        public Vector2 Size { get; }
        public Vector2 MaxRoomSize { get; }
        public Vector2 MinRoomSize { get; }
        public Vector2 CorridorMinSize { get; }
        public Vector3 playerStartPos;

        internal float rootDungeonSplit;
        internal float enemySpawnRate;
        internal float itemSpawnRate;
        internal int minEnemiesPerRoom;
        internal int maxEnemiesPerRoom;

        public int MaxRooms { get; }

        internal int maxItemsPerRoom;
        internal int minItemsPerRoom;


        /// <summary>
        /// Constructs dungeon parameters.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="minRoomSize"></param>
        /// <param name="maxRoomSize"></param>
        /// <param name="minCorridorSize"></param>
        /// <param name="enemySpawnRate"></param>
        /// <param name="itemSpawnRate"></param>
        /// <param name="rootDungeonSplit"></param>
        /// <param name="minItemsPerRoom"></param>
        /// <param name="maxItemsPerRoom"></param>
        /// <param name="minEnemiesPerRoom"></param>
        /// <param name="maxEnemiesPerRoom"></param>
        public DungeonParameters(Vector2 size, Vector2 minRoomSize, Vector2 maxRoomSize, Vector2 minCorridorSize, float enemySpawnRate, float itemSpawnRate, float rootDungeonSplit, int minItemsPerRoom, int maxItemsPerRoom, int minEnemiesPerRoom, int maxEnemiesPerRoom, int maxRooms) : this()
        {
            Size = size;
            MaxRoomSize = maxRoomSize;
            MinRoomSize = minRoomSize;
            CorridorMinSize = minCorridorSize;
            this.enemySpawnRate = enemySpawnRate;
            this.itemSpawnRate = itemSpawnRate;
            this.minItemsPerRoom = minItemsPerRoom;
            this.maxItemsPerRoom = maxItemsPerRoom;
            this.rootDungeonSplit = rootDungeonSplit;
            this.minEnemiesPerRoom = minEnemiesPerRoom;
            this.maxEnemiesPerRoom = maxEnemiesPerRoom;
            MaxRooms = maxRooms;
        }

        /// <summary>
        /// Constructs dungeon parameters from a JSON file.
        /// </summary>
        /// <param name="parametersFile">A JSON file with the needed parameters within.</param>
        public DungeonParameters(string filename) : this()
        {
            TextAsset jsonFile = Resources.Load<TextAsset>(filename);
            this = JsonUtility.FromJson<DungeonParameters>(jsonFile.text);
        }
    }
}