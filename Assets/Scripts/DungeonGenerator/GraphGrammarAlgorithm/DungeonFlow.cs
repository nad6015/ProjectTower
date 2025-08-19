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

            
        }
    }
}