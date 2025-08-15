using Assets.DungeonGenerator;
using Assets.DungeonGenerator.Components;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GraphGrammarTests
{
    readonly DungeonComponents components = Resources.Load<DungeonComponents>("DevComponents");
    readonly TextAsset paramFile = Resources.Load<TextAsset>("TestParameters");
    Dungeon dungeon;
    GraphGrammar algorithm;

    [SetUp]
    public void SetUp()
    {
        SceneManager.LoadScene("Scenes/Tests/RandomWalkAlgorithm");
        DungeonNode.Reset();
    }

    [Test]
    public void ShouldLoadBaseDungeon()
    {
        TestSetUp();

        DungeonLayout rooms = dungeon.Flow.FlowTemplate;

        Assert.That(rooms.Count == 3);

        DungeonNode firstNode = rooms.FirstNode;
        DungeonNode secondNode = rooms[firstNode][0];
        DungeonNode thirdNode = rooms[secondNode][1];

        Assert.That(firstNode.Type == RoomType.START);
        Assert.That(secondNode.Type == RoomType.EXPLORE);
        Assert.That(thirdNode.Type == RoomType.END);
    }

    [Test]
    public void ShouldRewriteBaseDungeon()
    {
        TestSetUp(5);

        DungeonLayout rooms = dungeon.Flow.FlowTemplate;
        Assert.That(rooms.Count == 3);

        algorithm.GenerateDungeon(dungeon);

        rooms = dungeon.Layout;

        DungeonNode firstNode = rooms.FirstNode;
        DungeonNode secondNode = rooms[firstNode][0];
        DungeonNode thirdNode = rooms[secondNode][1];
        DungeonNode fourthNode = rooms[thirdNode][1];
        DungeonNode fifthNode = rooms[fourthNode][1];


        Assert.That(rooms.Count == 5);

        Assert.That(firstNode.Type == RoomType.START);
        Assert.That(secondNode.Type == RoomType.EXPLORE);
        Assert.That(thirdNode.Type == RoomType.COMBAT);
        Assert.That(fourthNode.Type == RoomType.ITEM);
        Assert.That(fifthNode.Type == RoomType.END);

        Assert.That(rooms.FirstNode.Type == RoomType.START);
        Assert.That(rooms.LastNode.Type == RoomType.END);
    }

    private void TestSetUp(int roomCount = 2)
    {
        DungeonRepresentation parameters = new DungeonRepresentation(paramFile);
        parameters.ModifyParameter(DungeonParameter.ROOM_COUNT,
            new ValueRepresentation(ValueType.NUMBER, new() { { "value", roomCount.ToString() } }));

        dungeon = new(parameters, components);
        algorithm = new();
    }
}
