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

    private IEnumerator TestSetUp(int roomCount = 2)
    {
        Transform parent = GameObject.FindGameObjectWithTag("DungeonGenerator").transform;
        yield return new WaitForSeconds(1);

        DungeonParameters parameters = CreateParameters(roomCount);
        parameters.ModifyParameter("roomCount", roomCount);

        Dungeon dungeon = new(parameters, components);
        RandomWalk algorithm = new(parent);
        algorithm.GenerateDungeon(dungeon);

        yield return new WaitForSeconds(1);
    }

    private DungeonParameters CreateParameters(int maxRooms)
    {
        return new DungeonParameters(paramFile.name);
    }

    private List<DungeonRoom> FindRooms()
    {
        List<DungeonRoom> rooms = new(GameObject.FindObjectsByType<DungeonRoom>(FindObjectsSortMode.InstanceID));
        // Sort code referenced from - https://stackoverflow.com/questions/3498891/system-comparisont-understanding
        rooms.Sort((r1, r2) => r1.name.CompareTo(r2.name));
        return rooms;
    }
}
