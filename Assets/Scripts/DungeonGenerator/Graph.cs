using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Graphs;
using UnityEngine;


namespace Assets.DungeonGenerator
{
    public class Graph<T>
    {
        public int Count { get { return _graph.Count; } }

        // The internal data representation of the graph.
        // Each node T has a list of the nodes connected to it.
        private Dictionary<T, List<T>> _graph;

        /// <summary>
        /// Constructor. Creates an empty graph.
        /// </summary>
        public Graph()
        {
            _graph = new Dictionary<T, List<T>>();
        }

        /// <summary>
        /// Adds the specified node to the graph.
        /// If the node already exists in the graph, this is a no-op function.
        /// </summary>
        /// <param name="node"> the new node to add.</param>
        public void Add(T node)
        {
            if (!_graph.ContainsKey(node))
            {
                _graph[node] = new List<T>();
            }
        }

        /// <summary>
        /// Adds the specified node and it's connected nodes to the graph.
        /// If the specified node already exists in the graph, then this method only adds new nodes from the connected 
        /// nodes lists to the parent node's list.
        /// </summary>
        /// <param name="node">the new node to add.</param>
        /// <param name="connectedNodes">the nodes connected to this new node.</param>
        public void Add(T node, params T[] connectedNodes)
        {
            Add(node);

            List<T> nodes = _graph[node];

            foreach (var connectedNode in connectedNodes)
            {
                Add(connectedNode);
                if (!nodes.Contains(connectedNode))
                {
                    nodes.Add(connectedNode);
                }

                if (!_graph[connectedNode].Contains(node))
                {
                    _graph[connectedNode].Add(node);
                }
            }
        }

        /// <summary>
        /// Checks if a given node is in the graph.
        /// </summary>
        /// <param name="node"></param>
        /// <returns>true if the node is in the graph.</returns>
        public bool Contains(T node)
        {
            return _graph.ContainsKey(node);
        }

        /// <summary>
        /// Gets the neighbouring nodes to the specified node.
        /// </summary>
        /// <param name="node"></param>
        /// <returns>a list of the neighbouring nodes.</returns>
        public List<T> GetLinkedNodes(T node)
        {
            return _graph[node];
        }

        /// <summary>
        /// Checks if all nodes in the graph can be traversed. Uses the breadth first search (BFS)
        /// </summary>
        /// <returns>True if all nodes in the graphs can be traversed.</returns>
        public bool IsConnected()
        {
            if (Count == 0)
            {
                return false;
            }
            HashSet<T> visitedNodes = new();
            T firstNode = _graph.First().Key;
            visitedNodes.Add(firstNode);
            VisitNode(firstNode, visitedNodes);
            return visitedNodes.Count == Count;
        }

        private void VisitNode(T node, HashSet<T> visitedNodes)
        {
            foreach (var item in _graph[node])
            {
                if (visitedNodes.Contains(item))
                {
                    continue;
                }
                visitedNodes.Add(item);
                VisitNode(item, visitedNodes);
            }
        }

        public Dictionary<T, List<T>>.Enumerator GetEnumerator()
        {
            return _graph.GetEnumerator();
        }

        /// <summary>
        /// Allows direct indexing of the underlying dictionary. 
        /// Returns the specified node's linked nodes.
        /// </summary>
        public List<T> this[T key]
        {
            get
            {
                return _graph[key];
            }
        }

        internal void Remove(T node)
        {
            _graph.Remove(node);
        }
    }
}
