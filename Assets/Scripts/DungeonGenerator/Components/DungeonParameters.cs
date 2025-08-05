using Assets.Scripts.DungeonGenerator;
using Assets.Scripts.DungeonGenerator.DataStructures;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DungeonGenerator.Components
{
    /// <summary>
    /// Contains the parameters needed to generate a dungeon 
    /// </summary>
    public class DungeonParameters
    {
        public int Count { get { return _parameters.Count; } }

        private Dictionary<string, DungeonParameter> _parameters;

        /// <summary>
        /// Constructs dungeon parameters from a JSON file.
        /// </summary>
        /// <param name="parametersFile">A JSON file with the needed parameters within.</param>
        public DungeonParameters(string filename)
        {
            TextAsset jsonFile = Resources.Load<TextAsset>(filename);
            _parameters = new Dictionary<string, DungeonParameter>();

            JObject rulesJson = JObject.Parse(jsonFile.text);
            IEnumerable<JToken> jRules = rulesJson["params"].Children();

            foreach (JToken jRule in jRules)
            {
                DungeonParameter dungeonParameter = new(JsonConvert.DeserializeObject<Dictionary<string, object>>(jRule.ToString()));
                _parameters.Add(dungeonParameter.Id, dungeonParameter);
            }
        }

        public DungeonParameter GetParameter(string dungeonParams)
        {
            return _parameters[dungeonParams];
        }

        public void ModifyParameter(string paramName, int value)
        {
            _parameters[paramName].Modify(value);
        }
    }

    public struct Range<T>
    {
        public T min;
        public T max;

        public Range(T min, T max) : this()
        {
            this.min = min;
            this.max = max;
        }
    }
}