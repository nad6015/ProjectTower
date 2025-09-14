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
using Assets.PlayerCharacter.Resources;
using Assets.Interactables;
using Assets.DungeonGenerator.Components;

public class DungeonMasterTests
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
    public IEnumerator ShouldInitialiseSuccessfully()
    {
        TestSetUp();
        yield return new WaitForSeconds(1f);
        Assert.That(dungeonMaster.State == DungeonMasterState.Running);
        Assert.That(dungeonMaster.CurrentFloor == 1);

        Assert.NotNull(dungeonGenerator);
        Assert.That(dungeonGenerator.transform.childCount >= 10); // There should be at least 5 rooms + 5 corridors

        Assert.NotNull(FindGameObjectWithTag("Player"));

        yield return null;
    }

    [UnityTest]
    public IEnumerator ShouldGenerateNewDungeonOnClear()
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
    public IEnumerator ShouldLoadLoseSceneIfConditionsMet()
    {
        TestSetUp();

        yield return new WaitForSeconds(1f);
        Assert.That(dungeonMaster.State == DungeonMasterState.Running);

        player.GetComponent<TestPlayableFighter>().DefeatSelf();

        yield return new WaitForSeconds(1f);
        Assert.That(dungeonMaster.State == DungeonMasterState.GameEnd);
    }

    [UnityTest]
    public IEnumerator ShouldUseGameStateToModifyGameplayParameter()
    {
        TestSetUp();

        yield return new WaitForSeconds(1f);

        Assert.That(dungeonMaster.State == DungeonMasterState.Running);

        DestructibleItem item = GameObject.FindFirstObjectByType<DestructibleItem>();
        item.transform.SetPositionAndRotation(new(player.transform.position.x, 1, player.transform.position.z + 1), Quaternion.identity);
        player.GetComponent<TestPlayableFighter>().Attack();

        yield return new WaitForSeconds(1f);

        Assert.That(GameObject.FindFirstObjectByType<PickupItem>() == null);
        GameObject.Destroy(item);
        player.GetComponent<TestPlayableFighter>().DamageSelf(3); // Restoration drop rate should increase by 100
        yield return new WaitForSeconds(1f);

        DestructibleItem item2 = GameObject.FindFirstObjectByType<DestructibleItem>();
        item2.transform.SetPositionAndRotation(new(player.transform.position.x, 1, player.transform.position.z + 1), Quaternion.identity);
        player.GetComponent<TestPlayableFighter>().Attack();
        yield return new WaitForSeconds(1f);


        Assert.That(GameObject.FindFirstObjectByType<PickupItem>() != null);
    }

    [UnityTest]
    public IEnumerator ShouldUseGameStateToModifyDungeonParameter()
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
        Assert.That(GameObject.FindObjectsByType<TestNpcFighter>(FindObjectsSortMode.None).Length >= 5);
    }

    private void TestSetUp()
    {
        dungeonMaster = FindComponentByTag<DungeonMaster>("DungeonMaster");
        dungeonGenerator = FindComponentByTag<DungeonGenerator>("DungeonGenerator");
        player = FindGameObjectWithTag("Player");
    }
}
