using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.DungeonGenerator;
using Assets.DungeonGenerator.Components;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.TestTools.Utils;

public class RandomWalkTests
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

        Bounds room1 = rooms[0].Bounds;
        Bounds room2 = rooms[1].Bounds;

        Vector3 roomSize = new(25, 0, 25);
        Vector3Int dir = Vector3Int.RoundToInt((room2.min - room1.min).normalized);

        Assert.That(rooms.Count() == 2);
        Assert.That(corridors.Count() == 1);

        Assert.That(room1.size.magnitude <= roomSize.magnitude);
        Assert.That(room2.size.magnitude <= roomSize.magnitude);
    }

    [UnityTest]
    public IEnumerator ShouldCreateRightAlignedRooms()
    {
        yield return TestSetUp();

        List<DungeonRoom> rooms = FindRooms();

        Bounds room1 = rooms[0].Bounds;
        Bounds room2 = rooms[1].Bounds;

        Vector3Int dir = Vector3Int.RoundToInt((room2.min - room1.min).normalized);

        Assert.That(room1.center.z == 0);

        Assert.That(room2.center.x > room1.center.x);
        Assert.That(room2.center.z >= room1.min.z);
        Assert.That(room2.center.z <= room1.max.z);

        Assert.That(dir, Is.EqualTo(Vector3Int.right).Using(Vector3EqualityComparer.Instance));
    }

    [UnityTest]
    public IEnumerator ShouldCreateUpAlignedRooms()
    {
        yield return TestSetUp(DungeonAxis.VERTICAL);

        List<DungeonRoom> rooms = FindRooms();

        Bounds room1 = rooms[0].Bounds;
        Bounds room2 = rooms[1].Bounds;

        Vector3Int dir = Vector3Int.RoundToInt((room2.min - room1.min).normalized);

        Assert.That(room1.center.x == 0);

        Assert.That(room2.center.z > room1.center.z);
        Assert.That(room2.center.x >= room1.min.x);
        Assert.That(room2.center.x <= room1.max.x);

        Assert.That(dir, Is.EqualTo(Vector3Int.forward).Using(Vector3EqualityComparer.Instance));
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
        RandomWalk algorithm = new(dungeon, parent);
        algorithm.GenerateDungeon();

        yield return new WaitForSeconds(1);
    }

    private DungeonParameters CreateParameters(DungeonAxis axis, int maxRooms)
    {
        float dungeonSplit = 0;  // HORIZONTAL
        switch (axis)
        {
            case DungeonAxis.HORIZONTAL:
                dungeonSplit = 1;
                break;
            case DungeonAxis.VERTICAL:
                dungeonSplit = 0;
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
