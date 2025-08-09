using Assets.Scripts.DungeonGenerator.Components;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
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

            foreach (var node in jReplacer)
            {
                var json = JsonConvert.DeserializeObject<Dictionary<string, string>>(node.ToString());

                try
                {
                    RoomType roomType = RoomType.GENERIC;
                    Enum.Parse(typeof(RoomType), json["type"], true);
                    Replacer.Add(roomType);
                }
                catch
                {
                    Replacer.Add(RoomType.GENERIC);
                }
            }

            foreach (var node in jMatches)
            {
                Matches.Add(node.ToObject<RoomType>());
            }
        }
    }
}