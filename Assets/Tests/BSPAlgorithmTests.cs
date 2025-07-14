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
    public IEnumerator ShouldPartitionNode()
    {
        DungeonComponents components = GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<DungeonComponents>();
        yield return new WaitForSeconds(1);

        Dungeon dungeon = new(CreateParameters(), components);
        BSPAlgorithm algorithm = new(dungeon);
        algorithm.GenerateDungeon();

        yield return new WaitForSeconds(1);

        DungeonRoom[] rooms = GameObject.FindObjectsByType<DungeonRoom>(FindObjectsSortMode.None);
        DungeonCorridor[] corridors = GameObject.FindObjectsByType<DungeonCorridor>(FindObjectsSortMode.None);

        DungeonRoom room = GameObject.Find("Room 0").GetComponent<DungeonRoom>();

        Assert.That(rooms.Count() == 2);
        Assert.That(corridors.Count() == 1);

        Assert.NotNull(room);
        Assert.That(room.Contents.Count == 0);
    }

    [UnityTest]
    public IEnumerator ShouldPartitionNodeHorizontally()
    {
        DungeonComponents components = GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<DungeonComponents>();
        yield return new WaitForSeconds(1);

        Dungeon dungeon = new(CreateParameters(DungeonAxis.HORIZONTAL), components);
        BSPAlgorithm algorithm = new(dungeon);
        algorithm.GenerateDungeon();

        yield return new WaitForSeconds(1);

        DungeonRoom[] rooms = GameObject.FindObjectsByType<DungeonRoom>(FindObjectsSortMode.None);


        Assert.That(rooms[0].Bounds.x == 0);
        Assert.That(rooms[1].Bounds.x >= rooms[0].Bounds.xMax);

        Assert.That(rooms[0].Bounds.y == rooms[1].Bounds.y);
        Assert.That(rooms[0].Bounds.height == rooms[1].Bounds.height);
    }

    [UnityTest]
    public IEnumerator ShouldPartitionNodeVertically()
    {
        DungeonComponents components = GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<DungeonComponents>();
        yield return new WaitForSeconds(1);

        Dungeon dungeon = new(CreateParameters(DungeonAxis.VERTICAL), components);
        BSPAlgorithm algorithm = new(dungeon);
        algorithm.GenerateDungeon();

        yield return new WaitForSeconds(1);

        DungeonRoom[] rooms = GameObject.FindObjectsByType<DungeonRoom>(FindObjectsSortMode.None);


        Assert.That(rooms[0].Bounds.y == 0);
        Assert.That(rooms[1].Bounds.y >= rooms[0].Bounds.yMax);

        Assert.That(rooms[0].Bounds.x == rooms[1].Bounds.x);
        Assert.That(rooms[0].Bounds.width == rooms[1].Bounds.width);
    }

    private DungeonParameters CreateParameters()
    {
        return CreateParameters(0);
    }

    private DungeonParameters CreateParameters(DungeonAxis axis)
    {
        float dungeonSplit = 0;  // Vertical
        if (axis == DungeonAxis.HORIZONTAL)
        {
            dungeonSplit = 1;
        }
        return new DungeonParameters(new(50, 50), new(25, 25), new(25, 25), new(2, 2), 0, 0, dungeonSplit, 0, 0, 0, 0);
    }
}
