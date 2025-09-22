using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DungeonGenerator.Components
{
    /// <summary>
    /// A representation of a dungeon. Has its paramters set by the graph grammar and its components used by the random walk algorithm.
    /// </summary>
    public class DungeonRepresentation
    {
        public DungeonLayout Layout { get; private set; }
        public DungeonComponents Components { get; private set; }
        public List<FlowPattern> Flows { get; private set; }
        public int Count { get { return Parameters.Count; } }

        public Dictionary<DungeonParameter, ValueRepresentation> Parameters { get; private set; }
        private Dungeon _dungeon;

         public DungeonRepresentation(
                DungeonLayout dungeonLayout,
                List<FlowPattern> flowPatterns,
                DungeonComponents components,
                Dictionary<DungeonParameter,ValueRepresentation> parameters)
        {
            Layout = dungeonLayout;
            Flows = flowPatterns;
            Components = components;
            Parameters = parameters;
            _dungeon = new Dungeon();
        }

        public T Parameter<T>(DungeonParameter dungeonParams)
        {
            return Parameters[dungeonParams].Value<T>();
        }

        public void ModifyParameter(DungeonParameter paramName, ValueRepresentation value)
        {
            Parameters[paramName].Modify(value);
        }

        public void SetRooms(DungeonLayout dungeonRooms)
        {
            Layout = dungeonRooms;
        }

        internal void AddDungeonRoom(DungeonRoom room)
        {
            _dungeon.DungeonRooms.Add(room);
        }

        public Dungeon GetConstructedDungeon()
        {
            return _dungeon;
        }

        public void LoadFlows(List<FlowPattern> flowPatterns)
        {
            Flows.AddRange(flowPatterns);
        }

        internal int RandomItemCount()
        {
            Range<int> itemCountRange = Parameters[DungeonParameter.ItemsPerRoom].Value<Range<int>>();

            return UnityEngine.Random.Range(itemCountRange.min, itemCountRange.max);
        }
    }
}