using Assets.Scripts.DungeonGenerator.Components;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Assets.DungeonGenerator
{
    public class DungeonFlow
    {
        public Graph<DungeonFlowNode> FlowTemplate { get; }
        public List<FlowPattern> Flows { get; }

        public DungeonFlow(JToken baseFlow, JToken dungeonPatterns)
        {
            FlowTemplate = new();
            Flows = new();

            DungeonFlowNode lastRoom = null;

            foreach (JToken jNode in baseFlow.Children())
            {
                var json = JsonConvert.DeserializeObject<Dictionary<string, string>>(jNode.ToString());
                DungeonFlowNode room = new(json["type"]);

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

        internal bool ConditionMet()
        {
            return false;
        }
    }
}