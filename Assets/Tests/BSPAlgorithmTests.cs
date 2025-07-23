using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.DungeonGenerator;
using Assets.DungeonGenerator.Components;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class BSPAlgorithmTests
{
    DungeonComponents components = Resources.Load<DungeonComponents>("Dungeons/Dev/DevComponents");
    [SetUp]
    public void SetUp()
    {
        SceneManager.LoadScene("Scenes/Tests/BSPAlgorithm");
    }

    [UnityTest]
    public IEnumerator ShouldGenerateTwoRoomDungeon()
    {
        yield return TestSetUp();
        List<DungeonRoom> rooms = FindRooms();
        DungeonCorridor[] corridors = GameObject.FindObjectsByType<DungeonCorridor>(FindObjectsSortMode.None);

        Assert.That(rooms.Count() == 2);
        Assert.That(corridors.Count() == 1);

        Assert.That(rooms[0].rectBounds.size == new Vector2(25, 50));
        Assert.That(rooms[1].Contents.Count == 0);

        Assert.That(rooms[0].transform.childCount == (25 + 25 + 50 + 50));
        Assert.That(rooms[1].transform.childCount == (25 + 25 + 50 + 50));
    }

    [UnityTest]
    public IEnumerator ShouldGenerateHorizontallySplitRooms()
    {
        yield return TestSetUp();

        List<DungeonRoom> rooms = FindRooms();

        Assert.That(rooms[0].rectBounds.x == 0);
        Assert.That(rooms[0].rectBounds.width <= 25);
        Assert.That(rooms[1].rectBounds.x == 0);
        Assert.That(rooms[1].rectBounds.width <= 25);

        Assert.That(rooms[0].rectBounds.y == 0);
        Assert.That(rooms[0].rectBounds.height <= 25);

        Assert.That(rooms[1].rectBounds.y >= rooms[0].rectBounds.yMax);
        Assert.That(rooms[0].rectBounds.height >= rooms[1].rectBounds.height);
    }

    [UnityTest]
    public IEnumerator ShouldGenerateVerticallySplitRooms()
    {
        yield return TestSetUp(DungeonAxis.VERTICAL);

        List<DungeonRoom> rooms = FindRooms();

        Assert.That(rooms[0].rectBounds.y == 0);
        Assert.That(rooms[0].rectBounds.height <= 25);
        Assert.That(rooms[1].rectBounds.y >= rooms[0].rectBounds.yMax);
        Assert.That(rooms[1].rectBounds.height <= 25);

        Assert.That(rooms[0].rectBounds.x == 0);
        Assert.That(rooms[0].rectBounds.width <= 25);

        Assert.That(rooms[1].rectBounds.x >= rooms[0].rectBounds.xMax);
        Assert.That((rooms[0].rectBounds.width == rooms[1].rectBounds.width));
    }

    [UnityTest]
    public IEnumerator ShouldGenerateCompleteDungeon()
    {
        yield return TestSetUp(DungeonAxis.DEFAULT, 10);

        List<DungeonRoom> rooms = FindRooms();
        DungeonCorridor[] corridors = GameObject.FindObjectsByType<DungeonCorridor>(FindObjectsSortMode.None);

        Debug.Log(corridors.Length);

        Assert.That(rooms.Count == 10);
        Assert.That(corridors.Length >= 5);
    }

    private IEnumerator TestSetUp(DungeonAxis axis = DungeonAxis.HORIZONTAL, int maxRoomCount = 2)
    {
        Transform parent = GameObject.FindGameObjectWithTag("DungeonGenerator").transform;
        yield return new WaitForSeconds(1);

        Dungeon dungeon = new(CreateParameters(axis, maxRoomCount), components);
        BSPAlgorithm algorithm = new(dungeon, parent);
        algorithm.GenerateDungeon();

        yield return new WaitForSeconds(1);
    }

    private DungeonParameters CreateParameters(DungeonAxis axis, int maxRooms)
    {
        float dungeonSplit = 0;  // HORIZONTAL
        switch (axis)
        {
            case DungeonAxis.HORIZONTAL:
                dungeonSplit = 0;
                break;
            case DungeonAxis.VERTICAL:
                dungeonSplit = 1;
                break;
            default:
                dungeonSplit = 0.5f;
                break;
        }

        return new DungeonParameters(new Vector2(200, 200), new Vector2(15, 15), new Vector2(15, 15), new(2, 2), 0, 0, dungeonSplit, 0, 0, 0, 0, maxRooms);
    }

    private List<DungeonRoom> FindRooms()
    {
        List<DungeonRoom> rooms = new(GameObject.FindObjectsByType<DungeonRoom>(FindObjectsSortMode.InstanceID));
        // Sort code referenced from - https://stackoverflow.com/questions/3498891/system-comparisont-understanding
        rooms.Sort((r1, r2) => r1.name.CompareTo(r2.name));
        return rooms;
    }
}
