using System;
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
    [SetUp]
    public void SetUp()
    {
        SceneManager.LoadScene("Scenes/Tests/BSPAlgorithm");
    }

    [UnityTest]
    public IEnumerator ShouldGenerateTwoRoomDungeon()
    {
        DungeonComponents components = GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<DungeonComponents>();
        yield return new WaitForSeconds(1);

        Dungeon dungeon = new(CreateParameters(), components);
        BSPAlgorithm algorithm = new(dungeon);
        algorithm.GenerateDungeon();

        yield return new WaitForSeconds(1);

        List<DungeonRoom> rooms = FindRooms();
        DungeonCorridor[] corridors = GameObject.FindObjectsByType<DungeonCorridor>(FindObjectsSortMode.None);

        Assert.That(rooms.Count() == 2);
        Assert.That(corridors.Count() == 1);

        Assert.That(rooms[0].Bounds.size == new Vector2(25, 50));
        Assert.That(rooms[1].Contents.Count == 0);

        Assert.That(rooms[0].transform.childCount == (25 + 25 + 50 + 50));
        Assert.That(rooms[1].transform.childCount == (25 + 25 + 50 + 50));
    }

    [UnityTest]
    public IEnumerator ShouldGenerateHorizontallySplitRooms()
    {
        DungeonComponents components = GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<DungeonComponents>();
        yield return new WaitForSeconds(1);

        Dungeon dungeon = new(CreateParameters(DungeonAxis.HORIZONTAL), components);
        BSPAlgorithm algorithm = new(dungeon);
        algorithm.GenerateDungeon();

        yield return new WaitForSeconds(1);

        List<DungeonRoom> rooms = FindRooms();

        Assert.That(rooms[0].Bounds.x == 0);
        Assert.That(rooms[0].Bounds.width <= 25);
        Assert.That(rooms[1].Bounds.x == 0);
        Assert.That(rooms[1].Bounds.width <= 25);

        Assert.That(rooms[0].Bounds.y == 0);
        Assert.That(rooms[0].Bounds.height <= 25);

        Assert.That(rooms[1].Bounds.y >= rooms[0].Bounds.yMax);
        Assert.That(rooms[0].Bounds.height >= rooms[1].Bounds.height);
    }

    [UnityTest]
    public IEnumerator ShouldGenerateVerticallySplitRooms()
    {
        DungeonComponents components = GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<DungeonComponents>();
        yield return new WaitForSeconds(1);

        Dungeon dungeon = new(CreateParameters(DungeonAxis.VERTICAL), components);
        BSPAlgorithm algorithm = new(dungeon);
        algorithm.GenerateDungeon();

        yield return new WaitForSeconds(10);

        List<DungeonRoom> rooms = FindRooms();

        Assert.That(rooms[0].Bounds.y == 0);
        Assert.That(rooms[0].Bounds.height <= 25);
        Assert.That(rooms[1].Bounds.y >= rooms[0].Bounds.yMax);
        Assert.That(rooms[1].Bounds.height <= 25);

        Assert.That(rooms[0].Bounds.x == 0);
        Assert.That(rooms[0].Bounds.width <= 25);

        Assert.That(rooms[1].Bounds.x >= rooms[0].Bounds.xMax);
        Assert.That((rooms[0].Bounds.width == rooms[1].Bounds.width));
    }

    [UnityTest]
    public IEnumerator ShouldGenerateCompleteDungeon()
    {
        DungeonComponents components = GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<DungeonComponents>();
        yield return new WaitForSeconds(1);

        Dungeon dungeon = new(CreateParameters(10), components);
        BSPAlgorithm algorithm = new(dungeon);

        algorithm.GenerateDungeon();

        yield return new WaitForSeconds(1);

        List<DungeonRoom> rooms = FindRooms();
        DungeonCorridor[] corridors = GameObject.FindObjectsByType<DungeonCorridor>(FindObjectsSortMode.None);

        Debug.Log(corridors.Length);

        Assert.That(rooms.Count == 10);
        Assert.That(corridors.Length >= 5);
    }

    private DungeonParameters CreateParameters(int maxRooms = 2)
    {
        return CreateParameters(DungeonAxis.DEFAULT, maxRooms);
    }

    private DungeonParameters CreateParameters(DungeonAxis axis)
    {
        return CreateParameters(axis, 2);
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
        
        return new DungeonParameters(new Vector2(200,200), new Vector2(25, 25), new Vector2(25, 25), new(2, 2), 0, 0, dungeonSplit, 0, 0, 0, 0, maxRooms);
    }

    private List<DungeonRoom> FindRooms()
    {
        List<DungeonRoom> rooms = new(GameObject.FindObjectsByType<DungeonRoom>(FindObjectsSortMode.InstanceID));
        // Sort code referenced from - https://stackoverflow.com/questions/3498891/system-comparisont-understanding
        rooms.Sort((r1, r2) => r1.name.CompareTo(r2.name));
        return rooms;
    }
}
