using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.DungeonGenerator;
using Assets.DungeonGenerator.Components;
using Assets.DungeonMaster;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using static Assets.Utilities.GameObjectUtilities;

public class RandomWalkTests
{
    [SetUp]
    public void SetUp()
    {
        SceneManager.LoadScene("Scenes/Tests/RandomWalk");
        DungeonNode.Reset();
    }

    [UnityTest]
    public IEnumerator ShouldGenerateDungeonWithSpecifiedRoomNumber()
    {
        yield return TestSetUp();
        List<DungeonRoom> rooms = FindRooms();
        DungeonCorridor[] corridors = GameObject.FindObjectsByType<DungeonCorridor>(FindObjectsSortMode.None);

        Bounds room1 = rooms[0].Bounds;
        Bounds room2 = rooms[1].Bounds;

        Vector3 roomSize = new(25, 0, 25);

        Assert.That(rooms.Count() == 2);
        Assert.That(corridors.Count() == 1);

        Assert.That(room1.size.magnitude <= roomSize.magnitude);
        Assert.That(room2.size.magnitude <= roomSize.magnitude);
    }

    [UnityTest]
    public IEnumerator ShouldGenerateCompleteDungeon()
    {
        yield return TestSetUp(10);

        List<DungeonRoom> rooms = FindRooms();
        DungeonCorridor[] corridors = GameObject.FindObjectsByType<DungeonCorridor>(FindObjectsSortMode.None);

        Assert.That(rooms.Count == 10);
        Assert.That(corridors.Length >= 5);
    }

    [UnityTest]
    public IEnumerator ShouldCreateDungeonsBasedOnDungeonLayout()
    {
        yield return TestSetUp(3);

        List<DungeonRoom> rooms = FindRooms();

        Assert.That(rooms[0].Type == RoomType.Start);
        Assert.That(rooms[1].Type == RoomType.Explore);
        Assert.That(rooms[2].Type == RoomType.End);
    }

    [UnityTest]
    public IEnumerator ShouldCreateDungeonsWithRoomsThatBranch()
    {
        yield return TestSetUp(7, 1);

        List<DungeonRoom> rooms = FindRooms();

        DungeonNode start = null;
        DungeonNode end = null;

        int treasureCount = 0;
        int exploreCount = 0;

        foreach (var room in rooms)
        {
            switch (room.Type)
            {
                case RoomType.Treasure:
                {
                    treasureCount++;
                    break;
                }
                case RoomType.Explore:
                {
                    exploreCount++;
                    break;
                }
                case RoomType.End:
                {
                    end = room.DungeonNode;
                    break;
                }
                case RoomType.Start:
                {
                    start = room.DungeonNode;
                    break;
                }
            }
        }

        Assert.That(start != null);
        Assert.That(exploreCount == 5);
        Assert.That(treasureCount >= 1);
        Assert.That(end != null);
    }

    private IEnumerator TestSetUp(int roomCount = 2, int branchCount = 0)
    {
        DungeonGenerator dungeonGenerator = GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<DungeonGenerator>();

        dungeonGenerator.ClearDungeon();

        ParameterSupport support = FindComponentByTag<ParameterSupport>("TestSupport");

        DungeonRepresentation dungeon = new(null, null, support.Components,
            DungeonMasterDeserializationUtil.BuildDungeonParameters(JObject.Parse(support.ParamFile.text)));

        dungeon.ModifyParameter(DungeonParameter.RoomCount, new ValueRepresentation(ValueType.Number,
            new() { { "value", roomCount.ToString() } }));

        dungeon.SetRooms(CreateLayout(roomCount, branchCount));
        RandomWalk algorithm = new(dungeonGenerator.transform);
        algorithm.GenerateDungeon(dungeon);

        yield return new WaitForSeconds(1);
    }

    private List<DungeonRoom> FindRooms()
    {
        List<DungeonRoom> rooms = new(GameObject.FindObjectsByType<DungeonRoom>(FindObjectsSortMode.InstanceID));
        // Sort code referenced from - https://stackoverflow.com/questions/3498891/system-comparisont-understanding
        rooms.Sort((r1, r2) => r1.DungeonNode.Id.CompareTo(r2.DungeonNode.Id));
        return rooms;
    }

    private DungeonLayout CreateLayout(int roomCount, int branchCount)
    {
        DungeonNode.Reset();
        DungeonLayout layout = new();
        layout.Add(new(RoomType.Start));
        DungeonNode lastNode = layout.LastNode;
        int randomIndex1 = Random.Range(1, roomCount - 2);
        int randomIndex2 = Random.Range(1, roomCount - 2);

        for (int i = 1; i < roomCount - 1; i++)
        {
            DungeonNode node = new(RoomType.Explore);

            if (i == randomIndex1 || i == randomIndex2)
            {
                for (int j = 0; j < branchCount; j++)
                {

                    layout.Add(node, new DungeonNode(RoomType.Treasure));
                }
            }

            layout.Add(lastNode, node);
            lastNode = node;
        }

        layout.Add(lastNode, new DungeonNode(RoomType.End));
        return layout;
    }
}
