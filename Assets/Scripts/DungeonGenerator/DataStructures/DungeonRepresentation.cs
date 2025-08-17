using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DungeonGenerator.Components
{
    /// <summary>
    /// A representation of a dungeon. Has its paramters set by the graph grammar and its components used by the random walk algorithm.
    /// </summary>
    public class DungeonRepresentation
    {
        public int Count { get { return _parameters.Count; } }

        private readonly Dictionary<DungeonParameter, ValueRepresentation> _parameters;
        private readonly DungeonFlow _flow;
        public DungeonComponents Components { get; private set; }
        public DungeonLayout Layout { get; private set; }
        private Dungeon _dungeon;

        /// <summary>
        /// Constructs dungeon parameters from a JSON file.
        /// </summary>
        /// <param name="parametersFile">A JSON file with the needed parameters within.</param>
        public DungeonRepresentation(TextAsset file)
        {
            _dungeon = new Dungeon();
            _parameters = new Dictionary<DungeonParameter, ValueRepresentation>();
            Layout = new();

            JObject json = JObject.Parse(file.text);
            JsonUtils.ForEachIn(json["params"], jParam =>
            {
                DungeonParameter dungeonParameter = JsonUtils.ConvertToEnum<DungeonParameter>(jParam, "parameter");

                ValueType type = JsonUtils.ConvertToEnum<ValueType>(jParam["value"]);
                _parameters.Add(dungeonParameter, new ValueRepresentation(type, JsonUtils.ToDictionary(jParam["value"])));
            });

            _flow = new(json["baseDungeon"], json["dungeonPatterns"]);
        }

        public T Parameter<T>(DungeonParameter dungeonParams)
        {
            return _parameters[dungeonParams].Value<T>();
        }

        public void ModifyParameter(DungeonParameter paramName, ValueRepresentation value)
        {
            _parameters[paramName].Modify(value);
        }

        internal DungeonFlow GetFlow()
        {
            return _flow;
        }

        public void SetRooms(DungeonLayout dungeonRooms)
        {
            Layout = dungeonRooms;
        }

        public void SetComponents(DungeonComponents components)
        {
            Components = components;
        }

        internal void AddDungeonRoom(DungeonRoom room)
        {
            _dungeon.DungeonRooms.Add(room);
        }

        public Dungeon GetConstructedDungeon()
        {
            return _dungeon;
        }
    }
}