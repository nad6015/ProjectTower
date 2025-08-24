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
            DungeonLayout layout = dungeon.Layout;

            int roomCount = dungeon.Parameter<int>(DungeonParameter.RoomCount);
            Debug.Log(roomCount);

            while (layout.Count < roomCount)
            {
                foreach (var flow in dungeon.Flows)
                {
                    var rooms = layout.FindMatching(flow.Matches);
                    if (rooms.Count == flow.Matches.Count)
                    {
                        layout.Replace(rooms, flow.Replacer);
                    }

                    if(layout.Count >= roomCount)
                    {
                        break;
                    }
                }

                if (layout.LastNode.Type != RoomType.End)
                {
                    var end = layout.FindMatching(new List<RoomType>() { RoomType.End });

                    layout.Remove(end[0]);
                    layout.Add(layout.LastNode, new DungeonNode(RoomType.End));
                }
            }
        }
    }
}