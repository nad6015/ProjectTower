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

public class RandomWalkTests
{
    readonly DungeonComponents components = Resources.Load<DungeonComponents>("DevComponents");
    readonly TextAsset paramFile = Resources.Load<TextAsset>("TestParameters");

    [SetUp]
    public void SetUp()
    {
        SceneManager.LoadScene("Scenes/Tests/RandomWalkAlgorithm");
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

        Assert.That(rooms[0].Type == RoomType.START);
        Assert.That(rooms[1].Type == RoomType.EXPLORE);
        Assert.That(rooms[2].Type == RoomType.END);
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
                case RoomType.TREASURE:
                {
                    treasureCount++;
                    break;
                }
                case RoomType.EXPLORE:
                {
                    exploreCount++;
                    break;
                }
                case RoomType.END:
                {
                    end = room.DungeonNode;
                    break;
                }
                case RoomType.START:
                {
                    start = room.DungeonNode;
                    break;
                }
            }
        }

        yield return new WaitForSeconds(10f);

        Assert.That(start != null);
        Assert.That(exploreCount == 5);
        Assert.That(treasureCount == 1);
        Assert.That(end != null);
    }

    private IEnumerator TestSetUp(int roomCount = 2, int branchCount = 0)
    {
        DungeonGenerator dungeonGenerator = GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<DungeonGenerator>();
        
        dungeonGenerator.ClearDungeon();

        DungeonParameters parameters = new DungeonParameters(paramFile.name);
        parameters.ModifyParameter("roomCount", roomCount);
        Dungeon dungeon = new(parameters, components);

        dungeon.SetRooms(CreateLayout(roomCount, branchCount));
        RandomWalk algorithm = new(dungeonGenerator.transform);
        algorithm.GenerateDungeon(dungeon);

        yield return new WaitForSeconds(1);
    }

    private List<DungeonRoom> FindRooms()
    {
        List<DungeonRoom> rooms = new(GameObject.FindObjectsByType<DungeonRoom>(FindObjectsSortMode.InstanceID));
        // Sort code referenced from - https://stackoverflow.com/questions/3498891/system-comparisont-understanding
        rooms.Sort((r1, r2) => r1.name.CompareTo(r2.name));
        return rooms;
    }

    private DungeonLayout CreateLayout(int roomCount, int branchCount)
    {
        DungeonNode.Reset();
        DungeonLayout layout = new DungeonLayout();
        layout.Add(new DungeonNode(RoomType.START));
        DungeonNode lastNode = layout.LastNode;
        int randomIndex1 = Random.Range(1, roomCount - 2);
        int randomIndex2 = Random.Range(1, roomCount - 2); // TODO: Fix/Tidy up test

        for (int i = 1; i < roomCount - 1; i++)
        {
            DungeonNode node = new DungeonNode(RoomType.EXPLORE);

            if (i == randomIndex1 || i == randomIndex2)
            {
                for (int j = 0; j < branchCount; j++)
                {
                    
                    layout.Add(node, new DungeonNode(RoomType.TREASURE));
                }
            }

            layout.Add(lastNode, node);
            lastNode = node;
        }

        layout.Add(lastNode, new DungeonNode(RoomType.END));
        return layout;
    }
}
