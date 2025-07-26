using System.Collections;
using System.Collections.Generic;
using Assets.DungeonGenerator;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

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
        // Root game objects syhould be:
        //  Player
        //  Main camera
        //  At least one Directional Light
        //  Dungeon generator
        //  Dungeon Master
        //  Game Scene Manager (this is not counted in the rootCount because it is marked as don't destroy)
        Assert.That(SceneManager.GetActiveScene().rootCount == 5);
        yield return null;
    }

    [UnityTest]
    public IEnumerator ShouldClearAllDungeonComponents()
    {
        DungeonGenerator dungeonGenerator = GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<DungeonGenerator>();
        
        // Asserts that all root game objects are present
        Assert.That(SceneManager.GetActiveScene().rootCount == 5);
        Assert.That(dungeonGenerator.transform.childCount > 10);
        
        dungeonGenerator.ClearDungeon();
        
        yield return new WaitForSeconds(1f);

        // Asserts that all root game objects haven't been deleted alongside the dungeon components
        Assert.That(SceneManager.GetActiveScene().rootCount == 5);
        Assert.That(dungeonGenerator.transform.childCount == 0);
    }
}
