using Assets.Scripts.DungeonGenerator.Components;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Assets.DungeonGenerator
{
    public class FlowPattern
    {
        public List<DungeonNode> Matches { get; }
        public List<DungeonNode> Replacer { get; }

        public FlowPattern(JToken jMatches, JToken jReplacer)
        {
            Replacer = new();
            Matches = new();
            
            foreach (var node in jReplacer)
            {
                var json = JsonConvert.DeserializeObject<Dictionary<string, string>>(node.ToString());
                DungeonNode room = new(json["type"]);
                Replacer.Add(room);
            }

            foreach (var node in jMatches)
            {
                Matches.Add(node.ToObject<DungeonNode>());
            }
        }
    }
}