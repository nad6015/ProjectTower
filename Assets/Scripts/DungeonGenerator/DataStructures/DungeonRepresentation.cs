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
        public DungeonLayout Layout { get; private set; }
        public DungeonComponents Components { get; private set; }
        public List<FlowPattern> Flows { get; private set; }
        public int Count { get { return _parameters.Count; } }

        private readonly Dictionary<DungeonParameter, ValueRepresentation> _parameters;
        private Dungeon _dungeon;

        /// <summary>
        /// Constructs dungeon parameters from a JSON file.
        /// </summary>
        /// <param name="parametersFile">A JSON file with the needed parameters within.</param>
        public DungeonRepresentation(TextAsset file, DungeonComponents components)
        {
            Layout = new();
            _dungeon = new Dungeon();
            Flows = new List<FlowPattern>();
            _parameters = new Dictionary<DungeonParameter, ValueRepresentation>();
            Components = components;

            JObject json = JObject.Parse(file.text);
            JsonUtils.ForEachIn(json["params"], jParam =>
            {
                DungeonParameter dungeonParameter = jParam["parameter"].ToObject<DungeonParameter>();

                ValueType type = jParam["valueType"].ToObject<ValueType>();
                _parameters.Add(dungeonParameter, new ValueRepresentation(type, JsonUtils.ToDictionary(jParam["value"])));
            });

            DungeonNode lastRoom = null;

            JsonUtils.ForEachIn(json["baseDungeon"], (System.Action<JToken>)(jNode =>
            {
                DungeonNode room = new(jNode.ToObject<RoomType>());

                Layout.Add(room);

                if (lastRoom != null)
                {
                    Layout.Add(lastRoom, room);
                }

                lastRoom = room;
            }));

            JsonUtils.ForEachIn(json["dungeonPatterns"], pattern =>
            {
                Flows.Add(new FlowPattern(pattern["matches"], pattern["replacer"]));
            });
        }

        public T Parameter<T>(DungeonParameter dungeonParams)
        {
            return _parameters[dungeonParams].Value<T>();
        }

        public void ModifyParameter(DungeonParameter paramName, ValueRepresentation value)
        {
            _parameters[paramName].Modify(value);
        }

        public void SetRooms(DungeonLayout dungeonRooms)
        {
            Layout = dungeonRooms;
        }

        internal void AddDungeonRoom(DungeonRoom room)
        {
            _dungeon.DungeonRooms.Add(room);
        }

        public Dungeon GetConstructedDungeon()
        {
            return _dungeon;
        }

        public void LoadFlows(List<FlowPattern> flowPatterns)
        {
            Flows.AddRange(flowPatterns);
        }
    }
}