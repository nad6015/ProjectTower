using System.Collections;
using Assets.DungeonGenerator;
using Assets.DungeonMaster;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using static UnityEngine.GameObject;
using static Assets.Utilities.GameObjectUtilities;
using Tests.Support;
using Assets.Combat;
using Assets.DungeonGenerator.Components;

public class DungeonAdaptionTests
{
    DungeonMaster dungeonMaster;
    GameObject player;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        SceneManager.LoadScene("Scenes/Tests/DungeonMaster");
        yield return null;
    }

    [UnityTest]
    public IEnumerator ShouldVaryItemCountBasedOnDefeatedEnemyCount()
    {
        TestSetUp();

        yield return new WaitForSeconds(1f);

        Assert.That(dungeonMaster.State == DungeonMasterState.Running);
        Assert.That(GameObject.FindObjectsByType<DestructibleItem>(FindObjectsSortMode.None).Length <= 5);

        player.GetComponent<TestPlayableFighter>().DefeatRandomEnemy();

        yield return new WaitForSeconds(1f);

        dungeonMaster.OnDungeonCleared();

        yield return new WaitForSeconds(1f);

        Assert.That(dungeonMaster.State == DungeonMasterState.Running);
        Assert.That(dungeonMaster.CurrentFloor == 2);
        Assert.That(GameObject.FindObjectsByType<DestructibleItem>(FindObjectsSortMode.None).Length >= 10);

    }

    [UnityTest]
    public IEnumerator ShouldVaryEnemyCountBasedOnCharacterHealth()
    {
        TestSetUp();

        yield return new WaitForSeconds(1f);

        Assert.That(dungeonMaster.State == DungeonMasterState.Running);

        Assert.That(GameObject.FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length <= 5);
        player.GetComponent<TestPlayableFighter>().Heal(4);

        yield return new WaitForSeconds(1f);

        dungeonMaster.OnDungeonCleared();

        yield return new WaitForSeconds(1f);

        Assert.That(dungeonMaster.State == DungeonMasterState.Running);
        Assert.That(dungeonMaster.CurrentFloor == 2);
        Assert.That(GameObject.FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length >= 10);

        player.GetComponent<TestPlayableFighter>().DamageSelf(4);

        yield return new WaitForSeconds(1f);

        dungeonMaster.OnDungeonCleared();

        yield return new WaitForSeconds(1f);
        Assert.That(dungeonMaster.State == DungeonMasterState.Running);
        Assert.That(dungeonMaster.CurrentFloor == 3);
        Assert.That(GameObject.FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length <= 5);
    }

    private void TestSetUp()
    {
        dungeonMaster = FindComponentByTag<DungeonMaster>("DungeonMaster");
        player = FindGameObjectWithTag("Player");
    }
}
