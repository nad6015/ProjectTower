using Assets.DungeonGenerator.Components;
using Assets.Scripts.DungeonGenerator.Components;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DungeonGenerator
{
    public class GraphGrammar : IDungeonAlgorithm
    {
        private DungeonLayout _layout = new();

        /// <summary>
        /// <inheritdoc/>
        /// The algorithm for this generation is as such:
        /// 1. Create a graph representation of a dungeon using a base template.
        /// 2. Using the rules within the dungeon parameters, replace any graph nodes that match a rule's pattern with the rule's value.
        /// 3. Repeat step 2 until no matches are left or desired number of rooms are met.
        /// 4. Modify the dungeon to hold this information.
        /// </summary>
        /// <param name="dungeon">the dungeon instance</param>
        public void GenerateDungeon(Dungeon dungeon)
        {
            DungeonFlow dungeonFlow = dungeon.Flow;
            _layout = dungeonFlow.FlowTemplate;
            List<FlowPattern> flows = dungeonFlow.Flows;
            int loopCount = 0;
            int roomCount = Mathf.RoundToInt(dungeon.Parameter("roomCount").Value());

            while (_layout.Count < roomCount && loopCount < roomCount)
            {
                loopCount++;
                foreach (var flow in flows)
                {
                    var rooms = _layout.FindMatching(flow.Matches);
                    if (rooms.Count == flow.Matches.Count)
                    {
                        _layout.Replace(rooms, flow.Replacer);
                        loopCount = 0;
                    }
                    else
                    {
                        flow.Matches.ForEach(r => Debug.Log(r));
                    }
                }

                if (_layout.LastNode.Type != RoomType.END)
                {
                    var end = _layout.FindMatching(new List<RoomType>() { RoomType.END });
                    _layout.Remove(end[0]);
                    _layout.Add(_layout.LastNode, end[0]);
                }
            }
            dungeon.SetRooms(_layout);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void ClearDungeon()
        {
            _layout.RemoveAll();
        }
    }
}