using NUnit.Framework;
using Assets.DungeonGenerator;
using System.Collections.Generic;

public class GraphTests
{
    private readonly string node = "node";
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

    [Test]
    public void ShouldNotLinkedNodeWithItself()
    {
        graph.Add(node, node);

        Assert.That(graph.Count == 1);
    }

    [Test]
    public void ShouldNotAddNullValues()
    {
        graph.Add(null);

        Assert.That(graph.Count == 0);

        graph.Add(node, null);

        Assert.That(graph.Count == 1);

        graph.Add(null, null);

        Assert.That(graph.Count == 1);
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

    [Test]
    public void ShouldReturnFirstNodeOfMatchingPattern()
    {
        graph.Add("node", "node 1", "node 2");
        graph.Add("node 2", "node 3");
        graph.Add("node 3", "node 4");
        graph.Add("node 4", "node 5", "node 7");
        graph.Add("node 6", "node 7");
        graph.Add("node 7", "node 8");
        graph.Add("node 8", "node 9", "node 10");


        List<string> pattern = new()
        {
            "node 6",
            "node 7",
            "node 8"
        };

        List<string> foundNodes = graph.FindMatching(pattern);

        Assert.That(foundNodes.Count == 3);
        Assert.That(foundNodes.Contains("node 6"));
        Assert.That(foundNodes.Contains("node 7"));
        Assert.That(foundNodes.Contains("node 8"));
    }


    [Test]
    public void ShouldReplaceNodesMatchingAPattern()
    {
        graph = Create10NodeGraph();
        int count = graph.Count;

        List<string> pattern = new()
        {
            "node 6",
            "node 7",
            "node 8",
            "node 9"
        };

        List<string> replacer = new()
        {
            "node 11",
            "node 12",
            "node 13",
            "node 14"
        };

        List<string> foundNodes = graph.FindMatching(pattern);

        Assert.That(foundNodes.Count == 4);

        graph.Replace(foundNodes, replacer);

        Assert.That(graph.Count == count);
        Assert.That(!graph.Contains("node 6"));
        Assert.That(!graph.Contains("node 7"));
        Assert.That(!graph.Contains("node 8"));
        Assert.That(!graph.Contains("node 9"));

        Assert.That(graph.Contains("node 11"));
        Assert.That(graph.Contains("node 12"));
        Assert.That(graph.Contains("node 13"));
        Assert.That(graph.Contains("node 14"));

        Assert.That(graph.IsConnected());
    }

    [Test]
    public void ShouldReplaceNodesMatchingAPatternWithLessNodeThanPattern()
    {
        graph = Create10NodeGraph();
        int count = graph.Count;

        List<string> pattern = new()
        {
            "node 6",
            "node 7",
            "node 8"
        };

        List<string> replacer = new()
        {
            "node 11"
        };

        List<string> foundNodes = graph.FindMatching(pattern);

        Assert.That(foundNodes.Count == 3);

        graph.Replace(foundNodes, replacer);

        Assert.That(graph.Count == count - 2);

        Assert.That(!graph.Contains("node 6"));
        Assert.That(!graph.Contains("node 7"));
        Assert.That(!graph.Contains("node 8"));

        Assert.That(graph.Contains("node 11"));

        Assert.That(graph.IsConnected());
    }

    [Test]
    public void ShouldReplaceNodesMatchingAPatternWithMoreNodesThanPattern()
    {
        graph = Create10NodeGraph();
        int count = graph.Count;

        List<string> pattern = new()
        {
            "node 6",
            "node 7",
            "node 8",
            "node 9"
        };

        List<string> replacer = new()
        {
            "node 11",
            "node 12",
            "node 13",
            "node 14",
            "node 15",
            "node 16"
        };

        List<string> foundNodes = graph.FindMatching(pattern);

        graph.Replace(foundNodes, replacer);

        Assert.That(graph.Count == count + 2);

        Assert.That(!graph.Contains("node 6"));
        Assert.That(!graph.Contains("node 7"));
        Assert.That(!graph.Contains("node 8"));
        Assert.That(!graph.Contains("node 9"));

        Assert.That(graph.Contains("node 11"));
        Assert.That(graph.Contains("node 12"));
        Assert.That(graph.Contains("node 13"));
        Assert.That(graph.Contains("node 14"));
        Assert.That(graph.Contains("node 15"));
        Assert.That(graph.Contains("node 16"));

        Assert.That(graph.IsConnected());
    }

    public static IEnumerable<Graph<string>> CompleteGraphs()
    {
        Graph<string> singleGraph = new();
        singleGraph.Add("node");

        Graph<string> threeNodeGraph = new();
        threeNodeGraph.Add("node", "node 1", "node 2");

        yield return singleGraph;
        yield return threeNodeGraph;
        yield return Create10NodeGraph();
    }

    public static IEnumerable<Graph<string>> IncompleteGraphs()
    {
        Graph<string> emptyGraph = new();
        Graph<string> threeNodeGraph = new();
        threeNodeGraph.Add("node", "node 1");
        threeNodeGraph.Add("node 2");

        Graph<string> tenNodeGraph = new();
        tenNodeGraph.Add("node", "node 1", "node 2");
        tenNodeGraph.Add("node 2", "node 3");
        tenNodeGraph.Add("node 3", "node 4");
        tenNodeGraph.Add("node 4", "node 5", "node 7");
        tenNodeGraph.Add("node 6", "node 7");
        tenNodeGraph.Add("node 7", "node 8");
        tenNodeGraph.Add("node 9", "node 10");

        yield return emptyGraph;
        yield return threeNodeGraph;
        yield return Create10NodeGraph();
    }

    private static Graph<string> Create10NodeGraph()
    {
        Graph<string> tenNodeGraph = new();
        tenNodeGraph.Add("node", "node 1", "node 2");
        tenNodeGraph.Add("node 2", "node 3");
        tenNodeGraph.Add("node 3", "node 4");
        tenNodeGraph.Add("node 4", "node 5", "node 7");
        tenNodeGraph.Add("node 6", "node 7");
        tenNodeGraph.Add("node 7", "node 8");
        tenNodeGraph.Add("node 8", "node 9", "node 10");
        return tenNodeGraph;
    }
}
