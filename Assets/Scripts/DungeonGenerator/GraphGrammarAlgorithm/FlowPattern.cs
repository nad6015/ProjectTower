using Assets.DungeonGenerator.Components;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Assets.DungeonGenerator
{
    public class FlowPattern
    {
        public List<RoomType> Matches { get; }
        public List<RoomType> Replacer { get; }

        public FlowPattern(JToken jMatches, JToken jReplacer)
        {
            Replacer = new();
            Matches = new();

            foreach (var replacer in jReplacer)
            {
                Replacer.Add(replacer.ToObject<RoomType>());
            }

            foreach (var match in jMatches)
            {
                Matches.Add(match.ToObject<RoomType>());
            }
        }
    }
}