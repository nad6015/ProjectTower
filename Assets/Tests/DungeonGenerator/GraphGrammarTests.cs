using Assets.DungeonGenerator;
using Assets.DungeonGenerator.Components;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GraphGrammarTests
{
    readonly TextAsset paramFile = Resources.Load<TextAsset>("TestParameters");
    DungeonRepresentation dungeon;
    GraphGrammar algorithm;

    [SetUp]
    public void SetUp()
    {
        SceneManager.LoadScene("Scenes/Tests/RandomWalkAlgorithm");
        DungeonNode.Reset();
    }

    [Test]
    public void ShouldGenerateDungeonFromFlows()
    {
        TestSetUp(5);

        DungeonLayout rooms = dungeon.Layout;
        Assert.That(rooms.Count == 0);

        algorithm.GenerateDungeon(dungeon);

        rooms = dungeon.Layout;

        Assert.That(rooms.Count == 5);

        DungeonNode firstNode = rooms.FirstNode;
        DungeonNode secondNode = rooms[firstNode][0];
        DungeonNode thirdNode = rooms[secondNode][1];
        Debug.Log(rooms[thirdNode][0]);
        DungeonNode fourthNode = rooms[thirdNode][1];
        DungeonNode fifthNode = rooms[fourthNode][1];

        Assert.That(firstNode.Type == RoomType.Start);
        Assert.That(secondNode.Type == RoomType.Explore);
        Assert.That(thirdNode.Type == RoomType.Combat);
        Assert.That(fourthNode.Type == RoomType.Item);
        Assert.That(fifthNode.Type == RoomType.End);

        Assert.That(rooms.FirstNode.Type == RoomType.Start);
        Assert.That(rooms.LastNode.Type == RoomType.End);
    }

    private void TestSetUp(int roomCount = 2)
    {
        dungeon = new(paramFile);
        dungeon.ModifyParameter(DungeonParameter.RoomCount,
            new ValueRepresentation(ValueType.Number, new() { { "value", roomCount.ToString() } }));

        algorithm = new();
    }
}
