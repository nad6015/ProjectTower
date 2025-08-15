using UnityEngine;

namespace Assets.DungeonGenerator.Components
{
    /// <summary>
    /// A representation of a dungeon. Has its paramters set by the graph grammar and its components by the random walk algorithm.
    /// </summary>
    public class Dungeon
    {
        public DungeonExit DungeonExit { get; internal set; }
        public SpawnPoint StartingPoint { get; internal set; }
        public DungeonComponents Components { get; private set; }
        public Vector3 CorridorSize { get { return Parameter<Vector3>(DungeonParameter.CORRIDOR_SIZE); } }

        private readonly DungeonRepresentation _parameters;
        public DungeonLayout Layout { get; private set; }
        public DungeonFlow Flow { get { return _parameters.GetLayout(); } }

        /// <summary>
        /// Constructs a new dungeon.
        /// </summary>
        /// <param name="parameters">The parameters for this dungeon.</param>
        public Dungeon(DungeonRepresentation parameters, DungeonComponents components)
        {
            _parameters = parameters;
            Components = components;
            Layout = new();
        }

        public T Parameter<T>(DungeonParameter name)
        {
            return _parameters.GetParameter<T>(name);
        }

        public void SetRooms(DungeonLayout dungeonRooms)
        {
            Layout = dungeonRooms;
        }
    }
}