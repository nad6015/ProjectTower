using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assets.DungeonGenerator;
using System.Collections.Generic;

public class GraphTests
{
    private string node = "node";
    private Graph<string> graph;

    [SetUp]
    public void SetUp() { graph = new Graph<string>(); }

    [Test]
    public void ShouldAddNode()
    {
        graph.Add(node);
        Assert.That(graph.Contains(node));
    }

    [Test]
    public void ShouldAddNodeIdempotently()
    {
        graph.Add(node);
        Assert.That(graph.Count == 1);
        graph.Add(node);
        Assert.That(graph.Count == 1);
    }

    [Test]
    public void ShouldAddNodeWithLinkedNodes()
    {
        graph.Add(node, node + 1, node + 2);

        Assert.That(graph.Contains(node));
        Assert.That(graph.Contains(node + 1));
        Assert.That(graph.Contains(node + 2));

        Assert.That(graph[node].Count == 2);
        Assert.That(graph[node].Contains(node + 1));
        Assert.That(graph[node].Contains(node + 2));
    }

    [Test]
    public void ShouldAddNodeWithLinkedNodesIdempotently()
    {
        graph.Add(node, node + 1, node + 2);

        Assert.That(graph.Count == 3);
        Assert.That(graph[node].Count == 2);

        graph.Add(node, node + 1, node + 2);

        Assert.That(graph.Count == 3);
        Assert.That(graph[node].Count == 2);
    }

    [Test]
    public void ShouldAddNodeWithLinkToExistingNode()
    {
        graph.Add(node, node + 2);
        graph.Add(node + 1, node);

        Assert.That(graph.Contains(node));
        Assert.That(graph.Contains(node + 1));
        Assert.That(graph.Contains(node + 2));

        Assert.That(graph[node].Count == 2);
    }

    // TODO: Review graph terminology. I think better naming could be utilised.
    [TestCaseSource("CompleteGraphs")]
    public void ShouldReturnIsConnectedTrueIfGraphIsFullyLinked(Graph<string> graph)
    {
        Assert.True(graph.IsConnected());
    }

    // TODO: Review graph terminology. I think better naming could be utilised.
    [TestCaseSource("IncompleteGraphs")]
    public void ShouldReturnIsConnectedFalseIfGraphIsNotFullyLinked(Graph<string> graph)
    {
        Assert.False(graph.IsConnected());
    }

    public static IEnumerable<Graph<string>> CompleteGraphs()
    {
        Graph<string> singleGraph = new Graph<string>();
        singleGraph.Add("node");

        Graph<string> threeNodeGraph = new Graph<string>();
        threeNodeGraph.Add("node", "node 1", "node 2");

        Graph<string> tenNodeGraph = new Graph<string>();
        tenNodeGraph.Add("node", "node 1", "node 2");
        tenNodeGraph.Add("node 2", "node 3");
        tenNodeGraph.Add("node 3", "node 4");
        tenNodeGraph.Add("node 4", "node 5", "node 7");
        tenNodeGraph.Add("node 6", "node 7");
        tenNodeGraph.Add("node 7", "node 8");
        tenNodeGraph.Add("node 8", "node 9", "node 10");

        yield return singleGraph;
        yield return threeNodeGraph;
        yield return tenNodeGraph;
    }

    public static IEnumerable<Graph<string>> IncompleteGraphs()
    {
        Graph<string> emptyGraph = new Graph<string>();
        Graph<string> threeNodeGraph = new Graph<string>();
        threeNodeGraph.Add("node", "node 1");
        threeNodeGraph.Add("node 2");

        Graph<string> tenNodeGraph = new Graph<string>();
        tenNodeGraph.Add("node", "node 1", "node 2");
        tenNodeGraph.Add("node 2", "node 3");
        tenNodeGraph.Add("node 3", "node 4");
        tenNodeGraph.Add("node 4", "node 5", "node 7");
        tenNodeGraph.Add("node 6", "node 7");
        tenNodeGraph.Add("node 7", "node 8");
        tenNodeGraph.Add("node 9", "node 10");

        yield return emptyGraph;
        yield return threeNodeGraph;
        yield return tenNodeGraph;
    }
}
