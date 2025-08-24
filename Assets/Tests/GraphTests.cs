using NUnit.Framework;
using Assets.DungeonGenerator;
using System.Collections.Generic;
using Assets.DungeonGenerator.Components;

public class GraphTests
{
    private readonly DungeonNode node = new(RoomType.Generic);
    private DungeonLayout graph;

    [SetUp]
    public void SetUp()
    {
        DungeonNode.Reset();
        graph = new DungeonLayout();
    }

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
        var node2 = new DungeonNode(RoomType.Explore);
        var end = new DungeonNode(RoomType.End);
        graph.Add(node, node2, end);

        Assert.That(graph.Contains(node));
        Assert.That(graph.Contains(node2));
        Assert.That(graph.Contains(end));

        Assert.That(graph[node].Count == 2);
        Assert.That(graph[node].Contains(node2));
        Assert.That(graph[node].Contains(end));
    }

    [Test]
    public void ShouldAddNodeWithLinkedNodesIdempotently()
    {
        var node2 = new DungeonNode(RoomType.Explore);
        var end = new DungeonNode(RoomType.End);
        graph.Add(node, node2, end);

        Assert.That(graph.Count == 3);
        Assert.That(graph[node].Count == 2);

        graph.Add(node, node2, end);

        Assert.That(graph.Count == 3);
        Assert.That(graph[node].Count == 2);
    }

    [Test]
    public void ShouldAddNodeWithLinkToExistingNode()
    {
        var node2 = new DungeonNode(RoomType.Explore);
        var end = new DungeonNode(RoomType.End);

        graph.Add(node, end);
        graph.Add(node2, node);

        Assert.That(graph.Contains(node));
        Assert.That(graph.Contains(node2));
        Assert.That(graph.Contains(end));

        Assert.That(graph[node].Count == 2);
    }

    [Test]
    public void ShouldNotLinkNodeWithItself()
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
    // TODO: Add tests that ensures all node have at most three neighbours

    // TODO: Review graph terminology. I think better naming could be utilised.
    [TestCaseSource("CompleteGraphs")]
    public void ShouldReturnIsConnectedTrueIfGraphIsFullyLinked(DungeonLayout graph)
    {
        Assert.True(graph.IsConnected());
    }

    // TODO: Review graph terminology. I think better naming could be utilised.
    [TestCaseSource("IncompleteGraphs")]
    public void ShouldReturnIsConnectedFalseIfGraphIsNotFullyLinked(DungeonLayout graph)
    {
        Assert.False(graph.IsConnected());
    }

    [Test]
    public void ShouldReturnFirstNodeOfMatchingPattern()
    {
        graph = Create10NodeGraph();

        List<RoomType> pattern = new()
        {
            RoomType.Combat, RoomType.Explore, RoomType.Item
        };

        List<DungeonNode> foundNodes = graph.FindMatching(pattern);

        Assert.That(foundNodes.Count == 3);
        Assert.That(foundNodes[0].IsSameType(RoomType.Combat));
        Assert.That(foundNodes[1].IsSameType(RoomType.Explore));
        Assert.That(foundNodes[2].IsSameType(RoomType.Item));
    }


    [Test]
    public void ShouldReplaceNodesMatchingAPattern()
    {
        graph = Create10NodeGraph();
        int count = graph.Count;

        List<RoomType> pattern = new()
        {
            RoomType.Combat, RoomType.Explore, RoomType.Item, RoomType.Boss
        };

        List<RoomType> replacer = new()
        {
            RoomType.Combat, RoomType.Ambush,RoomType.Treasure,RoomType.Boss
        };

        List<DungeonNode> foundNodes = graph.FindMatching(pattern);

        Assert.That(foundNodes.Count == 4);

        graph.Replace(foundNodes, replacer);

        Assert.That(graph.Count == count);
        Assert.That(graph.FindById(5).IsSameType(RoomType.Combat));
        Assert.That(graph.FindById(6).IsSameType(RoomType.Ambush));
        Assert.That(graph.FindById(7).IsSameType(RoomType.Treasure));
        Assert.That(graph.FindById(8).IsSameType(RoomType.Boss));

        Assert.That(graph.IsConnected());
    }

    [Test]
    public void ShouldReplaceNodesMatchingAPatternWithLessNodeThanPattern()
    {
        graph = Create10NodeGraph();
        int count = graph.Count;

        List<RoomType> pattern = new()
        {
            RoomType.Combat, RoomType.Explore, RoomType.Item
        };

        List<RoomType> replacer = new() { RoomType.Treasure };

        List<DungeonNode> foundNodes = graph.FindMatching(pattern);

        Assert.That(foundNodes.Count == 3);

        graph.Replace(foundNodes, replacer);

        Assert.That(graph.Count == count - 2);

        Assert.That(graph.FindById(3).IsSameType(RoomType.Treasure));

        Assert.That(graph.IsConnected());
    }

    [Test]
    public void ShouldReplaceNodesMatchingAPatternWithMoreNodesThanPattern()
    {
        graph = Create10NodeGraph();
        int count = graph.Count;

        List<RoomType> pattern = new()
        {
            RoomType.Item, RoomType.Explore, RoomType.Combat, RoomType.RestPoint
        };

        List<RoomType> replacer = new()
        {
            RoomType.Combat, RoomType.Ambush, RoomType.Treasure,
            RoomType.Boss, RoomType.RestPoint, RoomType.Combat
        };

        List<DungeonNode> foundNodes = graph.FindMatching(pattern);

        Assert.That(foundNodes.Count == pattern.Count);

        graph.Replace(foundNodes, replacer);

        Assert.That(graph.Count == count + 2);

        Assert.That(graph.FindById(1).IsSameType(RoomType.Combat));
        Assert.That(graph.FindById(2).IsSameType(RoomType.Ambush));
        Assert.That(graph.FindById(3).IsSameType(RoomType.Treasure));
        Assert.That(graph.FindById(4).IsSameType(RoomType.Boss));
        Assert.That(graph.FindById(count).IsSameType(RoomType.RestPoint));
        Assert.That(graph.FindById(count + 1).IsSameType(RoomType.Combat));

        Assert.That(graph.IsConnected());
    }

    public static IEnumerable<DungeonLayout> CompleteGraphs()
    {
        DungeonLayout singleNodeGraph = new();
        singleNodeGraph.Add(new DungeonNode(RoomType.Generic));

        yield return singleNodeGraph;
        yield return Create3NodeGraph();
        yield return Create10NodeGraph();
    }

    public static IEnumerable<DungeonLayout> IncompleteGraphs()
    {
        DungeonLayout emptyGraph = new();
        DungeonLayout threeNodeGraph = Create3NodeGraph();

        threeNodeGraph.Remove(threeNodeGraph[threeNodeGraph.LastNode][0]);
        threeNodeGraph.Add(threeNodeGraph.LastNode, new DungeonNode(RoomType.Generic));

        DungeonLayout tenNodeGraph = Create10NodeGraph();
        threeNodeGraph.Remove(tenNodeGraph[tenNodeGraph.LastNode][0].LinkedNodes[0]);
        threeNodeGraph.Add(tenNodeGraph.LastNode, new DungeonNode(RoomType.Generic));

        yield return emptyGraph;
        yield return threeNodeGraph;
        yield return tenNodeGraph;
    }

    private static DungeonLayout Create3NodeGraph()
    {
        DungeonLayout threeNodeGraph = new();
        var start = new DungeonNode(RoomType.Start);
        var node2 = new DungeonNode(RoomType.Item);
        var end = new DungeonNode(RoomType.End);

        threeNodeGraph.Add(start, node2);
        threeNodeGraph.Add(node2, end);
        return threeNodeGraph;
    }

    private static DungeonLayout Create10NodeGraph()
    {
        DungeonLayout tenNodeGraph = new();
        var start = new DungeonNode(RoomType.Start);
        var node2 = new DungeonNode(RoomType.Item);
        var node3 = new DungeonNode(RoomType.Explore);
        var node4 = new DungeonNode(RoomType.Combat);
        var node5 = new DungeonNode(RoomType.RestPoint);
        var node6 = new DungeonNode(RoomType.Combat);
        var node7 = new DungeonNode(RoomType.Explore);
        var node8 = new DungeonNode(RoomType.Item);
        var node9 = new DungeonNode(RoomType.Boss);
        var end = new DungeonNode(RoomType.End);

        tenNodeGraph.Add(start, node2);
        tenNodeGraph.Add(node2, node3);
        tenNodeGraph.Add(node3, node4);
        tenNodeGraph.Add(node4, node5, node7);
        tenNodeGraph.Add(node6, node7);
        tenNodeGraph.Add(node7, node8);
        tenNodeGraph.Add(node8, node9, end);
        return tenNodeGraph;
    }
}
