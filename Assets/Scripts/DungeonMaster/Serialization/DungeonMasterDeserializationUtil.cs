using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Assets.DungeonGenerator.Components;
using Assets.DungeonGenerator;
using UnityEngine;

namespace Assets.DungeonMaster
{
    /// <summary>
    /// A static helper class for reading the dungeon master's configuration from a Json file.
    /// </summary>
    public static class DungeonMasterDeserializationUtil
    {
        public static Dictionary<string, DungeonRule> BuildDungeonRuleset(JObject json)
        {
            Dictionary<string, DungeonRule> _rules = new();

            JsonUtils.ForEachIn(json["dungeonRules"], jRule =>
            {
                string id = jRule["id"].ToString();
                if (!_rules.ContainsKey(id))
                {
                    DungeonParameter dungeonParameter = jRule["parameter"].ToObject<DungeonParameter>();
                    GameParameter gameParameter = jRule["gameParam"].ToObject<GameParameter>();

                    ValueRepresentation value = new(
                        jRule["valueType"].ToObject<ValueType>(),
                        JsonUtils.ToDictionary(jRule["value"]));

                    _rules[id] = new(id, dungeonParameter, gameParameter, BuildConditions(jRule), value);
                }
            });

            return _rules;
        }

        public static Dictionary<string, GameplayRule> BuildGameplayRuleset(JObject json)
        {
            Dictionary<string, GameplayRule> _rules = new();

            JsonUtils.ForEachIn(json["gameplayRules"], jRule =>
           {
               string id = jRule["id"].ToString();
               if (!_rules.ContainsKey(id))
               {
                   GameplayParameter gameplayParameter = JsonUtils.ConvertToEnum<GameplayParameter>(jRule, "parameter");
                   ValueRepresentation value = new(
                       JsonUtils.ConvertToEnum<ValueType>(jRule["valueType"]),
                        JsonUtils.ToDictionary(jRule["value"]));
                   GameParameter gameParameter = JsonUtils.ConvertToEnum<GameParameter>(jRule, "gameParam");

                   _rules[id] = new(id, gameplayParameter, gameParameter, BuildConditions(jRule), value);
               }
           });
            return _rules;
        }

        /// <summary>
        /// Reads the dungeon generator configuration from a json file and returns it as a
        /// DungeonMasterConfiguration object.
        /// </summary>
        /// <param name="configFile">the json file containing the configuration</param>
        /// <returns>A DungeonMasterConfiguration instance</returns>
        public static DungeonMasterConfiguration ReadGeneratorConfigFromJson(TextAsset configFile)
        {
            DungeonMasterConfiguration config = new()
            {
                DungeonFlows = new(),
                BaseDungeons = new()
            };

            JObject jFlows = JObject.Parse(configFile.text);
            JsonUtils.ForEachIn(jFlows, dungeonFlow =>
            {
                var dungeonFlowChildren = dungeonFlow.Children();
                List<FlowPattern> flowPatterns = new();
                DungeonLayout layout = new();

                DungeonMission mission = JsonUtils.ConvertToEnum<DungeonMission>(dungeonFlow.Path);

                foreach (var pattern in dungeonFlowChildren["flows"].Children())
                {
                    flowPatterns.Add(new FlowPattern(pattern["matches"], pattern["replacer"]));
                }

                DungeonNode lastRoom = null;

                foreach (var jNode in dungeonFlowChildren["baseDungeon"].Children())
                {
                    DungeonNode room = new(jNode.ToObject<RoomType>());

                    layout.Add(room);

                    if (lastRoom != null)
                    {
                        layout.Add(lastRoom, room);
                    }

                    lastRoom = room;
                }

                config.DungeonFlows.Add(mission, flowPatterns);
                config.BaseDungeons.Add(mission, layout);
            });

            return config;
        }

        /// <summary>
        /// Creates a dictionary of dungeon parameters used in the dungeon generation adaption.
        /// </summary>
        /// <param name="json">the json object containing the dungeon parameters</param>
        /// <returns>A Dictionary of dungeon parameters and their values</returns>
        public static Dictionary<DungeonParameter, ValueRepresentation> BuildDungeonParameters(JObject json)
        {
            Dictionary<DungeonParameter, ValueRepresentation> parameters = new();

            JsonUtils.ForEachIn(json["dungeonParams"], jParam =>
            {
                DungeonParameter dungeonParameter = jParam["parameter"].ToObject<DungeonParameter>();

                ValueType type = jParam["valueType"].ToObject<ValueType>();
                parameters.Add(dungeonParameter, new ValueRepresentation(type, JsonUtils.ToDictionary(jParam["value"])));
            });
            return parameters;
        }

        /// <summary>
        /// Creates a dictionary of gameplay parameters used in the gameplay adaption.
        /// </summary>
        /// <param name="json">the json object containing the gameplay parameters</param>
        /// <returns>A Dictionary of gameplay parameters and their values</returns>
        public static Dictionary<GameplayParameter, ValueRepresentation> BuildGameplayParameters(JObject json)
        {
            Dictionary<GameplayParameter, ValueRepresentation> parameters = new();

            JsonUtils.ForEachIn(json["gameplayParams"], jParam =>
            {
                GameplayParameter gameplayParam = jParam["parameter"].ToObject<GameplayParameter>();

                ValueType type = jParam["valueType"].ToObject<ValueType>();
                parameters.Add(gameplayParam, new ValueRepresentation(type, JsonUtils.ToDictionary(jParam["value"])));
            });
            return parameters;
        }

        private static List<ICondition> BuildConditions(JToken j)
        {
            List<ICondition> conditions = new();
            JsonUtils.ForEachIn(j["conditions"], jCondition =>
            {
                ICondition condition = null;
                switch (jCondition["operator"].ToString())
                {
                    case ">":
                    {
                        condition = new GreaterThanCondition(JsonUtils.ToInt(jCondition["operand"]));
                        break;
                    }
                    case "<":
                    {
                        condition = new LessThanCondition(JsonUtils.ToInt(jCondition["operand"]));
                        break;
                    }
                }
                if (condition != null)
                {
                    conditions.Add(condition);
                }
            });
            return conditions;
        }
    }
}