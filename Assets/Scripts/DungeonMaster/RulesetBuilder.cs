using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Assets.DungeonGenerator.Components;
using Assets.DungeonGenerator;

namespace Assets.DungeonMaster
{
    public class RulesetBuilder
    {
        public static Dictionary<DungeonParameter, DungeonRule> BuildDungeonRuleset(JObject json)
        {
            Dictionary<DungeonParameter, DungeonRule> _rules = new();

            JsonUtils.ForEachIn(json["dungeonRules"], jRule =>
            {
                DungeonParameter dungeonParameter = JsonUtils.ConvertToEnum<DungeonParameter>(jRule, "parameter");
                
                if (!_rules.ContainsKey(dungeonParameter))
                {
                    ValueRepresentation value = new(
                        JsonUtils.ConvertToEnum<ValueType>(jRule["value"]["type"]),
                        JsonUtils.ToDictionary(jRule["value"]));
                    GameParameter gameParameter = JsonUtils.ConvertToEnum<GameParameter>(jRule, "gameParam");

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