using Assets.DungeonGenerator.Components;
using Assets.Scripts.DungeonGenerator.Components;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.DungeonGenerator
{
    public class GraphGrammar : IDungeonAlgorithm
    {
        private Graph<DungeonFlowNode> _dungeonRooms = new();

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
            _dungeonRooms = dungeonFlow.FlowTemplate;
            List<FlowPattern> flows = dungeonFlow.Flows;

            while (_dungeonRooms.Count < dungeon.Parameter("roomCount").Value()) // TODO: Condition is met
            {
                foreach (var flow in flows)
                {
                    var rooms = _dungeonRooms.FindMatching(flow.Matches);
                    if (rooms.Count == flow.Matches.Count)
                    {
                        _dungeonRooms.Replace(rooms, flow.Replacer);
                    }
                }

                if (_dungeonRooms.LastNode.Type != RoomType.END)
                {
                    Debug.Log(_dungeonRooms.Count);
                    var end = _dungeonRooms.FindMatching(new List<DungeonFlowNode>() { new("end") });
                    _dungeonRooms.Remove(end[0]);
                    _dungeonRooms.Add(_dungeonRooms.LastNode, end[0]);
                }
            }
            dungeon.SetRooms(_dungeonRooms);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void ClearDungeon()
        {
            _dungeonRooms.RemoveAll();
        }
    }
}