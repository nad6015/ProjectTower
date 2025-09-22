using System.Collections;
using Assets.DungeonGenerator;
using Assets.DungeonMaster;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using static Assets.Utilities.GameObjectUtilities;

public class DungeonGeneratorTest
{
    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("Scenes/Tests/DungeonGenerator");
    }

    [UnityTest]
    public IEnumerator ShouldContainAllDungeonComponents()
    {
        // Asserts that all root game objects are present
        // Root game objects should be:
        //  Player
        //  Main camera
        //  At least one Directional Light
        //  The game systems prefab
        yield return new WaitForSeconds(1f);
        Assert.That(SceneManager.GetActiveScene().rootCount == 4);
    }

    [UnityTest]
    public IEnumerator ShouldClearAllDungeonComponents()
    {
        DungeonGenerator dungeonGenerator = FindComponentByTag<DungeonGenerator>("DungeonGenerator");
        DungeonMaster dungeonMaster = FindComponentByTag<DungeonMaster>("DungeonMaster");
        dungeonMaster.enabled = false;

        // Asserts that all root game objects are present
        Assert.That(SceneManager.GetActiveScene().rootCount == 4);
        Assert.That(dungeonGenerator.transform.childCount > 10);
        
        dungeonGenerator.ClearDungeon();
        
        yield return new WaitForSeconds(1f);

        // Asserts that all root game objects haven't been deleted alongside the dungeon components
        Assert.That(SceneManager.GetActiveScene().rootCount == 4);
        Assert.That(dungeonGenerator.transform.childCount == 0);
    }
}
