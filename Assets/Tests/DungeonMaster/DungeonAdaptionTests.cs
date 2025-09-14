using System.Collections;
using Assets.DungeonGenerator;
using Assets.DungeonMaster;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.TestTools.Utils;
using static UnityEngine.GameObject;
using static Assets.Utilities.GameObjectUtilities;
using Assets.PlayerCharacter;
using Tests.Support;
using Assets.Combat;

public class DungeonAdaptionTests
{
    DungeonMaster dungeonMaster;
    GameObject player;
    DungeonGenerator dungeonGenerator;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        SceneManager.LoadScene("Scenes/Tests/DungeonMaster");
        yield return null;
    }

    [UnityTest]
    public IEnumerator ShouldAdaptNextDungeonBasedOnGameData()
    {
        TestSetUp();

        yield return new WaitForSeconds(1f);

        Assert.That(dungeonMaster.State == DungeonMasterState.Running);

        player.GetComponent<TestPlayableFighter>().DefeatRandomEnemy();

        yield return new WaitForSeconds(1f);

        dungeonMaster.OnDungeonCleared();

        yield return new WaitForSeconds(1f);

        Assert.That(dungeonMaster.State == DungeonMasterState.Running);
        Assert.That(dungeonMaster.CurrentFloor == 2);
        Assert.That(GameObject.FindObjectsByType<NpcFighter>(FindObjectsSortMode.None).Length >= 5);
    }

    [UnityTest]
    public IEnumerator ShouldIncreaseDungeonParameterValueIfRuleIsMet()
    {
        TestSetUp();

        yield return new WaitForSeconds(1f);

        Assert.That(dungeonMaster.State == DungeonMasterState.Running);
        Assert.That(dungeonMaster.CurrentFloor == 1);

        GameObject startPos = FindGameObjectWithTag("PlayerSpawn");

        Vector3 startPosition = startPos.transform.position;

        Assert.NotNull(startPos);
        Assert.NotNull(player);

        yield return new WaitForSeconds(1f);

        dungeonMaster.OnDungeonCleared();

        player = FindGameObjectWithTag("Player");
        Assert.That(dungeonMaster.CurrentFloor == 1);
        Assert.That(dungeonMaster.State == DungeonMasterState.GenerateDungeon);
        Assert.NotNull(player);
        Assert.That(FindComponentByTag<PlayerController>("Player").enabled == false); // PlayerController should not enabled while the dungeon is generating

        yield return new WaitForSeconds(1f);

        GameObject newStartPos = FindGameObjectWithTag("PlayerSpawn");
        player = FindGameObjectWithTag("Player");

        Assert.That(dungeonMaster.State == DungeonMasterState.Running);
        Assert.That(dungeonMaster.CurrentFloor == 2);

        Assert.NotNull(newStartPos);
        Assert.NotNull(player);
        Assert.That(newStartPos.transform.position, Is.Not.EqualTo(startPosition).Using(Vector3EqualityComparer.Instance));
    }

    [UnityTest]
    public IEnumerator ShouldCollectFloorTimeStatsAndIncreaseDungeonParameter()
    {
        TestSetUp();

        yield return new WaitForSeconds(1f);

        Assert.That(dungeonMaster.State == DungeonMasterState.Running);
        Assert.That(dungeonMaster.CurrentFloor == 1);

        GameObject startPos = FindGameObjectWithTag("PlayerSpawn");

        Vector3 startPosition = startPos.transform.position;

        Assert.NotNull(startPos);
        Assert.NotNull(player);

        yield return new WaitForSeconds(1f);

        dungeonMaster.OnDungeonCleared();

        player = FindGameObjectWithTag("Player");
        Assert.That(dungeonMaster.CurrentFloor == 1);
        Assert.That(dungeonMaster.State == DungeonMasterState.GenerateDungeon);
        Assert.NotNull(player);
        Assert.That(FindComponentByTag<PlayerController>("Player").enabled == false); // PlayerController should not enabled while the dungeon is generating

        yield return new WaitForSeconds(1f);

        GameObject newStartPos = FindGameObjectWithTag("PlayerSpawn");
        player = FindGameObjectWithTag("Player");

        Assert.That(dungeonMaster.State == DungeonMasterState.Running);
        Assert.That(dungeonMaster.CurrentFloor == 2);

        Assert.NotNull(newStartPos);
        Assert.NotNull(player);
        Assert.That(newStartPos.transform.position, Is.Not.EqualTo(startPosition).Using(Vector3EqualityComparer.Instance));
    }

    [UnityTest]
    public IEnumerator ShouldCollectEnemiesDefeatedAndIncreaseDungeonParameter()
    {
        TestSetUp();

        yield return new WaitForSeconds(1f);

        Assert.That(dungeonMaster.State == DungeonMasterState.Running);
        Assert.That(dungeonMaster.CurrentFloor == 1);

        GameObject startPos = FindGameObjectWithTag("PlayerSpawn");

        Vector3 startPosition = startPos.transform.position;

        Assert.NotNull(startPos);
        Assert.NotNull(player);

        yield return new WaitForSeconds(1f);

        dungeonMaster.OnDungeonCleared();

        player = FindGameObjectWithTag("Player");
        Assert.That(dungeonMaster.CurrentFloor == 1);
        Assert.That(dungeonMaster.State == DungeonMasterState.GenerateDungeon);
        Assert.NotNull(player);
        Assert.That(FindComponentByTag<PlayerController>("Player").enabled == false); // PlayerController should not enabled while the dungeon is generating

        yield return new WaitForSeconds(1f);

        GameObject newStartPos = FindGameObjectWithTag("PlayerSpawn");
        player = FindGameObjectWithTag("Player");

        Assert.That(dungeonMaster.State == DungeonMasterState.Running);
        Assert.That(dungeonMaster.CurrentFloor == 2);

        Assert.NotNull(newStartPos);
        Assert.NotNull(player);
        Assert.That(newStartPos.transform.position, Is.Not.EqualTo(startPosition).Using(Vector3EqualityComparer.Instance));
    }

    [UnityTest]
    public IEnumerator ShouldCollectCharacterHealthAndIncreaseDungeonParameter()
    {
        TestSetUp();

        yield return new WaitForSeconds(1f);

        Assert.That(dungeonMaster.State == DungeonMasterState.Running);
        Assert.That(dungeonMaster.CurrentFloor == 1);

        GameObject startPos = FindGameObjectWithTag("PlayerSpawn");

        Vector3 startPosition = startPos.transform.position;

        Assert.NotNull(startPos);
        Assert.NotNull(player);

        yield return new WaitForSeconds(1f);

        dungeonMaster.OnDungeonCleared();

        player = FindGameObjectWithTag("Player");
        Assert.That(dungeonMaster.CurrentFloor == 1);
        Assert.That(dungeonMaster.State == DungeonMasterState.GenerateDungeon);
        Assert.NotNull(player);
        Assert.That(FindComponentByTag<PlayerController>("Player").enabled == false); // PlayerController should not enabled while the dungeon is generating

        yield return new WaitForSeconds(1f);

        GameObject newStartPos = FindGameObjectWithTag("PlayerSpawn");
        player = FindGameObjectWithTag("Player");

        Assert.That(dungeonMaster.State == DungeonMasterState.Running);
        Assert.That(dungeonMaster.CurrentFloor == 2);

        Assert.NotNull(newStartPos);
        Assert.NotNull(player);
        Assert.That(newStartPos.transform.position, Is.Not.EqualTo(startPosition).Using(Vector3EqualityComparer.Instance));
    }

    private void TestSetUp()
    {
        dungeonMaster = FindComponentByTag<DungeonMaster>("DungeonMaster");
        dungeonGenerator = FindComponentByTag<DungeonGenerator>("DungeonGenerator");
        player = FindGameObjectWithTag("Player");
    }
}
