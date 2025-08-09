using Assets.Scripts.DungeonGenerator.Components;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Assets.DungeonGenerator
{
    public class Graph<T>
    {
        public int Count { get { return _graph.Count; } }

        public T FirstNode { get; private set; }
        public T LastNode { get; private set; }

        // The internal data representation of the graph.
        // Each node T has a list of the nodes connected to it.
        private readonly Dictionary<T, List<T>> _graph;

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
            // TODO: TEST FOR NULL NODE
            if (node != null && !_graph.ContainsKey(node))
            {
                _graph[node] = new List<T>();

                if (FirstNode == null)
                {
                    FirstNode = node;
                }
                else
                {
                    LastNode = node;
                }
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
            if (node == null)
            {
                return;
            }

            Add(node);

            if (connectedNodes == null)
            {
                return;
            }

            List<T> nodes = _graph[node];

            foreach (var connectedNode in connectedNodes)
            {
                Add(connectedNode);
                if (connectedNode != null && !nodes.Contains(connectedNode))
                {
                    nodes.Add(connectedNode);
                }

                if (connectedNode != null && !_graph[connectedNode].Contains(node))
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

        /// <summary>
        /// Removes all the nodes in the graph.
        /// </summary>
        public void RemoveAll()
        {
            _graph.Clear();
        }

        /// <summary>
        /// Removes a node in the graph.
        /// </summary>
        /// <param name="node"></param>
        public void Remove(T node)
        {
            foreach (T child in _graph[node])
            {
                _graph[child].Remove(node);
            }
            _graph.Remove(node); // TODO: Test that all instances of the node is removed from grpah
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

        /// <summary>
        /// Finds and returns the first node of a set matching the given list of nodes.
        /// </summary>
        /// <param name="pattern">the node pattern to match</param>
        /// <returns>the first node of the matching set</returns>
        public List<T> FindMatching(List<T> pattern)
        {
            List<T> matchingPattern = new();
            T lastNodeInPattern = default;

            foreach (var node in _graph.Keys)
            {
                for (int j = 0; j < pattern.Count; j++)
                {
                    T nextNodeToSearchFor = pattern[j];

                    if (node.Equals(nextNodeToSearchFor) && matchingPattern.Count == 0)
                    {
                        lastNodeInPattern = node;
                        matchingPattern.Add(node);
                    }
                    else if (matchingPattern.Count > 0 && _graph[lastNodeInPattern].Contains(nextNodeToSearchFor))
                    {
                        int index = _graph[lastNodeInPattern].IndexOf(nextNodeToSearchFor);

                        matchingPattern.Add(_graph[lastNodeInPattern][index]);
                        lastNodeInPattern = _graph[lastNodeInPattern][index];
                    }
                    else
                    {
                        matchingPattern.Clear();
                        lastNodeInPattern = default;
                        break;
                    }
                }

                if (matchingPattern.Count == pattern.Count)
                {
                    break;
                }
            }
            return matchingPattern;
        }

        /// <summary>
        /// Replaces the given nodes with the the nodes in the replacer list.
        /// If the number of replacement nodes are less than the nodes to replace, then this function does nothing.
        /// </summary>
        /// <param name="nodes">a set of nodes within this graph.</param>
        /// <param name="replacer">a set of nodes to replace them with.</param>
        public void Replace(List<T> nodes, List<T> replacer)
        {
            T lastNode = default;

            if (replacer.Count < nodes.Count)
            {
                return; // NO-OP
            }

            // Replaces the existing nodes
            for (int i = 0; i < nodes.Count; i++)
            {
                var item = nodes[i];
                var linkedNodes = _graph[item];

                for (int j = 0; j < linkedNodes.Count; j++)
                {
                    T node = linkedNodes[j];
                    if (_graph.ContainsKey(node))
                    {
                        _graph[node].Remove(item);
                    }
                    Add(node, replacer[i]);
                }

                linkedNodes.RemoveAll(n => nodes.Contains(n));

                _graph.Remove(item);

                Add(replacer[i], linkedNodes.ToArray());

                lastNode = replacer[i];
            }

            // Extends the existing nodes.
            if (replacer.Count > nodes.Count)
            {
                int index = replacer.IndexOf(lastNode) + 1;
                for (int i = index; i < replacer.Count; i++)
                {
                    Add(lastNode, replacer[i]);
                    lastNode = replacer[i];
                }
            }

            foreach (var item in _graph)
            {
                foreach (var i in item.Value)
                {
                    Debug.Log($"{item.Key}: {i}");
                }
            }
        }
    }
}
