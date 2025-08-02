using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Linq;
using Newtonsoft.Json;

namespace Assets.DungeonGenerator
{
    public class DungeonMasterRuleset
    {
        public int Count { get { return _rules.Count; } }

        private readonly Dictionary<string, DungeonMasterRule> _rules;
        public DungeonMasterRuleset(string filename)
        {
            TextAsset jsonFile = Resources.Load<TextAsset>(filename);
            _rules = new();

            // Newtonsoft.Json code referenced from - https://www.newtonsoft.com/json/help/html/SerializingJSONFragments.htm
            JObject rulesJson = JObject.Parse(jsonFile.text);
            IList<JToken> jRules = rulesJson["rules"].Children().ToList();

            foreach (JToken jRule in jRules)
            {
                List<RuleCondition> conditions = new List<RuleCondition>();
                
                foreach (JToken jCondition in jRule["conditions"])
                {
                    conditions.Add(jCondition.ToObject<RuleCondition>());
                }

                RuleValue ruleValue = new(JsonConvert.DeserializeObject<Dictionary<string, object>>(jRule["value"].ToString()));

                DungeonMasterRule rule = new(jRule["id"].ToString(), jRule["parameter"].ToString(), conditions, ruleValue);
                if (!_rules.ContainsKey(rule.Id))
                {
                    _rules.Add(rule.Id, rule);
                }
            }
        }

        /// <summary>
        /// Loops through all the rules in this ruleset.
        /// </summary>
        /// <param name="action">the method to run on each rule</param>
        public void ForEach(System.Action<DungeonMasterRule> action)
        {
            foreach (var rule in _rules)
            {
                action(rule.Value);
            }
        }
    }
}