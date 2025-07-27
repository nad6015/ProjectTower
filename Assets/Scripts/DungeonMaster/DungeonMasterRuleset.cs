using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Linq;

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
                DungeonMasterRule rule = jRule.ToObject<DungeonMasterRule>();
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