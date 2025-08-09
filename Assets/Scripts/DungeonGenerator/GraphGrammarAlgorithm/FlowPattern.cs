using Assets.Scripts.DungeonGenerator.Components;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Assets.DungeonGenerator
{
    public class FlowPattern
    {
        public List<DungeonFlowNode> Matches { get; }
        public List<DungeonFlowNode> Replacer { get; }

        public FlowPattern(JToken jMatches, JToken jReplacer)
        {
            Replacer = new();
            Matches = new();
            
            foreach (var node in jReplacer)
            {
                var json = JsonConvert.DeserializeObject<Dictionary<string, string>>(node.ToString());
                DungeonFlowNode room = new(json["type"]);
                Replacer.Add(room);
            }

            foreach (var node in jMatches)
            {
                Matches.Add(node.ToObject<DungeonFlowNode>());
            }
        }
    }
}