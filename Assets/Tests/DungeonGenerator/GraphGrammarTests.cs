using Assets.DungeonGenerator;
using Assets.DungeonGenerator.Components;
using Assets.DungeonMaster;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class GraphGrammarTests
{
    DungeonRepresentation dungeon;
    GraphGrammar algorithm;
    const int startingRoomCount = 5;

    [SetUp]
    public void SetUp()
    {
        SceneManager.LoadScene("Scenes/Tests/GraphGrammar");
        DungeonNode.Reset();
    }

    [UnityTest]
    public IEnumerator ShouldLoadBaseDungeonWhenRoomCountMatches()
    {
        yield return TestSetUp(3);
        DungeonLayout rooms = dungeon.Layout;
        Assert.That(rooms.Count == startingRoomCount);

        algorithm.GenerateDungeon(dungeon);

        rooms = dungeon.Layout;

        Assert.That(rooms.Count == startingRoomCount);

        DungeonNode firstNode = rooms.FirstNode;
        DungeonNode secondNode = rooms[firstNode][0];
        DungeonNode thirdNode = rooms[secondNode][1];
        DungeonNode fourthNode = rooms[thirdNode][1];
        DungeonNode fifthNode = rooms[fourthNode][1];

        Assert.That(firstNode.Type == RoomType.Start);
        Assert.That(secondNode.Type == RoomType.Explore);
        Assert.That(thirdNode.Type == RoomType.Combat);
        Assert.That(fourthNode.Type == RoomType.Treasure);
        Assert.That(fifthNode.Type == RoomType.End);

        Assert.That(rooms.FirstNode.Type == RoomType.Start);
        Assert.That(rooms.LastNode.Type == RoomType.End);
    }


    [UnityTest]
    public IEnumerator ShouldGenerateDungeonFromFlows()
    {
        yield return TestSetUp(5);

        DungeonLayout rooms = dungeon.Layout;
        Assert.That(rooms.Count == startingRoomCount);

        algorithm.GenerateDungeon(dungeon);

        rooms = dungeon.Layout;

        Assert.That(rooms.Count == 5);

        DungeonNode firstNode = rooms.FirstNode;
        DungeonNode secondNode = rooms[firstNode][0];
        DungeonNode thirdNode = rooms[secondNode][1];
        DungeonNode fourthNode = rooms[thirdNode][1];
        DungeonNode fifthNode = rooms[fourthNode][1];

        Assert.That(firstNode.Type == RoomType.Start);
        Assert.That(secondNode.Type == RoomType.Explore);
        Assert.That(thirdNode.Type == RoomType.Combat);
        Assert.That(fourthNode.Type == RoomType.Treasure);
        Assert.That(fifthNode.Type == RoomType.End);

        Assert.That(rooms.FirstNode.Type == RoomType.Start);
        Assert.That(rooms.LastNode.Type == RoomType.End);
    }

    [UnityTest]
    public IEnumerator ShouldGenerateConnectedDungeons([ValueSource("roomCounts")] int roomCount)
    {
        yield return TestSetUp(roomCount);

        DungeonLayout rooms = dungeon.Layout;
        Assert.That(rooms.Count == startingRoomCount);

        algorithm.GenerateDungeon(dungeon);

        rooms = dungeon.Layout;

        Assert.That(rooms.Count >= roomCount);
        Assert.That(rooms.IsConnected());
    }

    private IEnumerator TestSetUp(int roomCount = 2)
    {
        ParameterSupport support = GameObject.FindGameObjectWithTag("TestSupport").GetComponent<ParameterSupport>();
        TextAsset paramFile = support.ParamFile;
        TextAsset configFile = support.ConfigFile;

        var parameters = DungeonMasterDeserializationUtil.BuildDungeonParameters(paramFile);
        var config = DungeonMasterDeserializationUtil.ReadGeneratorConfigFromJson(configFile);
        
        dungeon = new(config.BaseDungeons[DungeonMission.ExploreFloor], config.DungeonFlows[DungeonMission.ExploreFloor], null, parameters);
        dungeon.ModifyParameter(DungeonParameter.RoomCount,
            new ValueRepresentation(ValueType.Number, new() { { "value", roomCount.ToString() } }));

        algorithm = new();
        yield return new WaitForSeconds(1);
    }

    static int[] roomCounts = new int[]
    {
        1, 2, 3, 5, 7, 10, 17, 32
    };
}
