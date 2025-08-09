using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.DungeonGenerator;
using Assets.DungeonGenerator.Components;
using Assets.Scripts.DungeonGenerator.Components;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

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

        Debug.Log(thirdNode.Type);

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

        rooms = dungeon.Rooms;

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
        DungeonParameters parameters = CreateParameters();
        parameters.ModifyParameter("roomCount", roomCount);

        dungeon = new(parameters, components);
        algorithm = new();
    }

    private DungeonParameters CreateParameters()
    {
        return new DungeonParameters(paramFile.name);
    }

}
