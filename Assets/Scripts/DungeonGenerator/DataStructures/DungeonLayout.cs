using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.DungeonGenerator.Components;

namespace Assets.DungeonGenerator
{
    public class DungeonLayout
    {
        public int Count { get { return _graph.Count; } }

        public DungeonNode FirstNode { get; private set; }
        public DungeonNode LastNode { get; private set; }

        // The internal data representation of the graph.
        // Each node T has a list of the nodes connected to it.
        private readonly List<DungeonNode> _graph;

        /// <summary>
        /// Constructor. Creates an empty graph.
        /// </summary>
        public DungeonLayout()
        {
            _graph = new();
        }

        /// <summary>
        /// Adds the specified node to the graph.
        /// If the node already exists in the graph, this is a no-op function.
        /// </summary>
        /// <param name="node"> the new node to add.</param>
        public void Add(DungeonNode node)
        {
            // TODO: TEST FOR NULL NODE
            if (node != null && !_graph.Contains(node))
            {
                _graph.Add(node);

                FirstNode ??= node;
                LastNode = node;
            }
        }

        /// <summary>
        /// Adds the specified node and it's connected nodes to the graph.
        /// If the specified node already exists in the graph, then this method only adds new nodes from the connected 
        /// nodes lists to the parent node's list.
        /// </summary>
        /// <param name="node">the new node to add.</param>
        /// <param name="nodesToConnect">the nodes connected to this new node.</param>
        public void Add(DungeonNode node, params DungeonNode[] nodesToConnect)
        {
            if (node == null)
            {
                return;
            }

            Add(node);
            List<DungeonNode> linkedNodes = node.LinkedNodes;

            if (nodesToConnect == null || linkedNodes.Count >= 3)
            {
                return;
            }

            foreach (var nodeToConnect in nodesToConnect)
            {  
                Add(nodeToConnect);
                if (nodeToConnect != null && !linkedNodes.Contains(nodeToConnect))
                {
                    linkedNodes.Add(nodeToConnect);
                }

                if (nodeToConnect != null && !nodeToConnect.LinkedNodes.Contains(node))
                {
                    nodeToConnect.LinkedNodes.Add(node);
                }
            }
        }

        /// <summary>
        /// Checks if a given node is in the graph.
        /// </summary>
        /// <param name="node"></param>
        /// <returns>true if the node is in the graph.</returns>
        public bool Contains(DungeonNode node)
        {
            return _graph.Contains(node);
        }

        /// <summary>
        /// Gets the neighbouring nodes to the specified node.
        /// </summary>
        /// <param name="node"></param>
        /// <returns>a list of the neighbouring nodes.</returns>
        public List<DungeonNode> GetLinkedNodes(DungeonNode node)
        {
            return node.LinkedNodes;
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
            HashSet<DungeonNode> visitedNodes = new();
            DungeonNode firstNode = _graph.First();
            visitedNodes.Add(firstNode);
            VisitNode(firstNode, visitedNodes);
            Debug.Log(visitedNodes.Count);
            Debug.Log(Count);
            return visitedNodes.Count == Count;
        }

        public List<DungeonNode>.Enumerator GetEnumerator()
        {
            return _graph.GetEnumerator();
        }

        /// <summary>
        /// Allows direct indexing of the underlying dictionary. 
        /// Returns the specified node's linked nodes.
        /// </summary>
        public List<DungeonNode> this[DungeonNode key]
        {
            get
            {
                return key.LinkedNodes;
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
        public void Remove(DungeonNode node)
        {
            foreach (DungeonNode child in node.LinkedNodes)
            {
                child.LinkedNodes.Remove(node);
            }
            _graph.Remove(node); // TODO: Test that all instances of the node is removed from grpah
        }

        private void VisitNode(DungeonNode node, HashSet<DungeonNode> visitedNodes)
        {
            foreach (var item in node.LinkedNodes)
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
        public List<DungeonNode> FindMatching(List<RoomType> pattern)
        {
            List<DungeonNode> matchingPattern = new();
            DungeonNode lastNodeInPattern = null;

            foreach (var node in _graph)
            {
                for (int j = 0; j < pattern.Count; j++)
                {
                    RoomType patternType = pattern[j];

                    if (node.IsSameType(patternType) && matchingPattern.Count == 0)
                    {
                        lastNodeInPattern = node;
                        matchingPattern.Add(node);
                    }
                    else if (matchingPattern.Count > 0 && null != lastNodeInPattern.LinkedNodes.Find(n => n.IsSameType(patternType)))
                    {
                        DungeonNode foundNode = lastNodeInPattern.LinkedNodes.Find(n => n.IsSameType(patternType));

                        matchingPattern.Add(foundNode);
                        lastNodeInPattern = foundNode;
                    }
                    else
                    {
                        matchingPattern.Clear();
                        lastNodeInPattern = null;
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
        public void Replace(List<DungeonNode> nodes, List<RoomType> replacer)
        {
            DungeonNode lastNode = default;
            int count = Mathf.Min(replacer.Count, nodes.Count);

            // Replaces the existing nodes
            for (int i = 0; i < count; i++)
            {
                var item = nodes[i];
                item.ChangeType(replacer[i]);
                lastNode = item;
            }

            // Extends the existing nodes.
            if (replacer.Count > nodes.Count)
            {
                for (int i = count; i < replacer.Count; i++)
                {
                    var newNode = new DungeonNode(replacer[i]);
                    Add(newNode, lastNode);
                    lastNode = newNode;
                }
            }
            else if (replacer.Count < nodes.Count)
            {
                List<DungeonNode> allNodes = new();
                for (int i = 0; i < nodes.Count; i++)
                {
                    var node = nodes[i];
                    var linkedNodes = node.LinkedNodes;
                    allNodes.AddRange(linkedNodes);

                    Remove(node);
                    allNodes.Remove(node);
                    lastNode.LinkedNodes.Remove(node);
                }

                Add(lastNode, allNodes.ToArray());
            }
        }

        //TODO: Write test for this method
        public DungeonNode FindById(int v)
        {
            return _graph.Find(n=>n.Id == v);
        }
    }
}
