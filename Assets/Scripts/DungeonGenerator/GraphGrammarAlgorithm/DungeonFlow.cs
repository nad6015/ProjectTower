using Assets.DungeonGenerator.Components;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Assets.DungeonGenerator
{
    public class DungeonFlow
    {
        public DungeonLayout FlowTemplate { get; }
        public List<FlowPattern> Flows { get; }

        public DungeonFlow(JToken baseFlow, JToken dungeonPatterns)
        {
            FlowTemplate = new();
            Flows = new();

            DungeonNode lastRoom = null;

            foreach (JToken jNode in baseFlow.Children())
            {
                DungeonNode room = new(JsonUtils.ConvertToEnum<RoomType>(jNode));

                FlowTemplate.Add(room);

                if (lastRoom != null)
                {
                    FlowTemplate.Add(lastRoom, room);
                }

                lastRoom = room;
            }

            foreach (JToken pattern in dungeonPatterns.Children())
            {
                Flows.Add(new FlowPattern(pattern["matches"], pattern["replacer"]));
            }
        }
    }
}