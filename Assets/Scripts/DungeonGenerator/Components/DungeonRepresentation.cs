using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DungeonGenerator.Components
{
    /// <summary>
    /// Represents the flow and attributes of a dungeon.
    /// </summary>
    public class DungeonRepresentation
    {
        public int Count { get { return _parameters.Count; } }

        private readonly Dictionary<DungeonParameter, ValueRepresentation> _parameters;
        private readonly DungeonFlow _layout;

        /// <summary>
        /// Constructs dungeon parameters from a JSON file.
        /// </summary>
        /// <param name="parametersFile">A JSON file with the needed parameters within.</param>
        public DungeonRepresentation(TextAsset file)
        {
            _parameters = new Dictionary<DungeonParameter, ValueRepresentation>();

            JObject json = JObject.Parse(file.text);
            JsonUtils.ForEachIn(json["params"], jParam =>
            {
                DungeonParameter dungeonParameter = JsonUtils.ConvertToEnum<DungeonParameter>(jParam, "id"); //TODO: Renameid to parameter

                ValueType type = JsonUtils.ConvertToEnum<ValueType>(jParam["value"]);
                _parameters.Add(dungeonParameter, new ValueRepresentation(type, JsonUtils.FlattenedJsonValues(jParam["value"])));
            });

            _layout = new(json["baseDungeon"], json["dungeonPatterns"]);
        }

        public T GetParameter<T>(DungeonParameter dungeonParams)
        {
            return _parameters[dungeonParams].Value<T>();
        }

        public void ModifyParameter(DungeonParameter paramName, ValueRepresentation value)
        {
            _parameters[paramName].Modify(value);
        }

        internal DungeonFlow GetLayout()
        {
            return _layout;
        }

    }
}