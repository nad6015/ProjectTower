using System;
using System.Collections.Generic;

namespace Assets.DungeonGenerator
{
    internal class Graph<T>
    {
        internal Dictionary<T, List<T>> GraphDict { get; private set; }

        public Graph()
        {
            GraphDict = new Dictionary<T, List<T>>();
        }

        public void Add(T item) { 
            GraphDict[item] = new List<T>();
        }

        public void Add(T item, params T[] items)
        {
            GraphDict[item] = new List<T>(items);
        }

        public List<T> GetNeighbors(T item) { 
            return GraphDict[item];
        }

        internal bool HasNoNeighbors(T firstRoom)
        {
            List<T> neighbours = GraphDict[firstRoom];
            return neighbours == null || neighbours.Count == 0;
        }

        internal void Remove(T firstRoom)
        {
            GraphDict.Remove(firstRoom);
        }
    }
}
