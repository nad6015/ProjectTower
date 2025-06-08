using UnityEngine;

namespace Assets.DungeonGenerator.Components
{
    /**
     * TODO
     */
    internal struct Dungeon
    {
        public Vector2 Size { get; set; }
        public Vector2 MaxRoomSize { get; private set; }
        public Vector2 MinRoomSize { get; private set; }

        /**
         * TODO:
         */
        public Dungeon(Vector2 size, Vector2 minRoomSize, Vector2 maxRoomSize) : this()
        {
            Size = size;
            MaxRoomSize = maxRoomSize;
            MinRoomSize = minRoomSize;
        }


    }
}