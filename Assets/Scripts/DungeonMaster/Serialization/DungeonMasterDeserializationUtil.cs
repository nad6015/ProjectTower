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
        public static Dictionary<DungeonParameter, DungeonRule> BuildDungeonRuleset(JObject json)
        {
            Dictionary<DungeonParameter, DungeonRule> _rules = new();

            JsonUtils.ForEachIn(json["dungeonRules"], jRule =>
            {
                DungeonParameter dungeonParameter = jRule["parameter"].ToObject<DungeonParameter>();

                if (!_rules.ContainsKey(dungeonParameter))
                {
                    GameParameter gameParameter = jRule["gameParam"].ToObject<GameParameter>();

                    ValueRepresentation value = new(
                        jRule["valueType"].ToObject<ValueType>(),
                        JsonUtils.ToDictionary(jRule["value"]));

                    _rules[dungeonParameter] = new DungeonRule(dungeonParameter, gameParameter, BuildConditions(jRule), value);
                }
            });

            return _rules;
        }

        public static Dictionary<GameplayParameter, GameplayRule> BuildGameplayParams(JObject json)
        {
            Dictionary<GameplayParameter, GameplayRule> _rules = new();

            JsonUtils.ForEachIn(json["gameplayRules"], jRule =>
           {
               GameplayParameter gameplayParameter = JsonUtils.ConvertToEnum<GameplayParameter>(jRule, "parameter");
               if (!_rules.ContainsKey(gameplayParameter))
               {
                   ValueRepresentation value = new(
                       JsonUtils.ConvertToEnum<ValueType>(jRule["valueType"]),
                        JsonUtils.ToDictionary(jRule["value"]));
                   GameParameter gameParameter = JsonUtils.ConvertToEnum<GameParameter>(jRule, "gameParam");

                   _rules[gameplayParameter] = new(gameplayParameter, gameParameter, BuildConditions(jRule), value);
               }
           });
            return _rules;
        }

        public static DungeonMasterConfiguration ReadConfigFromJson(TextAsset flowsFile)
        {
            DungeonMasterConfiguration config = new()
            {
                DungeonFlows = new()
            };

            JObject jFlows = JObject.Parse(flowsFile.text);
            JsonUtils.ForEachIn(jFlows, dungeonFlow =>
            {
                List<FlowPattern> flowPatterns = new();

                JsonUtils.ForEachIn(jFlows[dungeonFlow.Path], pattern =>
                {
                    flowPatterns.Add(new FlowPattern(pattern["matches"], pattern["replacer"]));
                });

                config.DungeonFlows.Add(JsonUtils.ConvertToEnum<DungeonMission>(dungeonFlow.Path), flowPatterns);
            });
            return config;
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