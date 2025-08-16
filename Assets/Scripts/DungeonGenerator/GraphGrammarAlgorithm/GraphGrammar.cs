using Assets.DungeonGenerator.Components;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DungeonGenerator
{
    public class GraphGrammar : IDungeonAlgorithm
    {
        /// <summary>
        /// <inheritdoc/>
        /// The algorithm for this generation is as such:
        /// 1. Create a graph representation of a dungeon using a base template.
        /// 2. Using the rules within the dungeon parameters, replace any graph nodes that match a rule's pattern with the rule's value.
        /// 3. Repeat step 2 until no matches are left or desired number of rooms are met.
        /// 4. Modify the dungeon to hold this information.
        /// </summary>
        /// <param name="dungeon">the dungeon representation</param>
        public void GenerateDungeon(DungeonRepresentation dungeon)
        {
            DungeonFlow dungeonFlow = dungeon.GetFlow();
            DungeonLayout _layout = dungeonFlow.FlowTemplate;
            List<FlowPattern> flows = dungeonFlow.Flows;

            int roomCount = dungeon.Parameter<int>(DungeonParameter.RoomCount);

            while (_layout.Count < roomCount)
            {
                foreach (var flow in flows)
                {
                    var rooms = _layout.FindMatching(flow.Matches);
                    if (rooms.Count == flow.Matches.Count)
                    {
                        _layout.Replace(rooms, flow.Replacer);
                    }
                    else
                    {
                        flow.Matches.ForEach(r => Debug.Log(r));
                    }
                }
        
                if (_layout.LastNode.Type != RoomType.End)
                {
                    var end = _layout.FindMatching(new List<RoomType>() { RoomType.End });
                    _layout.Remove(end[0]);
                    _layout.Add(_layout.LastNode, new DungeonNode(RoomType.End));
                }
            }
            dungeon.SetRooms(_layout);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void ClearDungeon()
        {
            //_layout.RemoveAll();
        }
    }
}