using System.Collections;
using Assets.DungeonGenerator;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.TestTools.Utils;

public class DungeonMasterTests
{
    DungeonMaster dungeonMaster;
    [UnitySetUp]
    public IEnumerator SetUp()
    {
        SceneManager.LoadScene("Scenes/Tests/DungeonMaster");
        yield return null;
    }

    [UnityTest]
    public IEnumerator ShouldInitialiseSuccessfully()
    {
        dungeonMaster = GameObject.FindGameObjectWithTag("DungeonMaster").GetComponent<DungeonMaster>();
        dungeonMaster.NewDungeon();
        Assert.That(dungeonMaster.State == DungeonMaster.DungeonMasterState.AWAITING_START);

        dungeonMaster.StartDungeonMaster();

        Assert.That(dungeonMaster.State == DungeonMaster.DungeonMasterState.ENTER_DUNGEON);

        yield return new WaitForSeconds(1f);

        Assert.That(dungeonMaster.State == DungeonMaster.DungeonMasterState.RUNNING);
        Assert.That(dungeonMaster.IsReady());
    }

    [UnityTest]
    public IEnumerator ShouldSetMonitoringTargets()
    {
        // TODO
        Assert.Fail();
        yield return null;
    }

    [UnityTest]
    public IEnumerator ShouldRegenerateDungeonOnClear()
    {
        GameObject startPos = GameObject.FindGameObjectWithTag("PlayerSpawn");
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        Assert.Null(startPos);
        Assert.Null(player);

        TestSetUp();


        Assert.That(dungeonMaster.State == DungeonMaster.DungeonMasterState.ENTER_DUNGEON);
        Assert.That(dungeonMaster.Floor == 0);

        yield return new WaitForSeconds(1f);

        Assert.That(dungeonMaster.State == DungeonMaster.DungeonMasterState.RUNNING);
        startPos = GameObject.FindGameObjectWithTag("PlayerSpawn");
        player = GameObject.FindGameObjectWithTag("Player");
        Vector3 startPosition = startPos.transform.position;
        Assert.NotNull(startPos);
        Assert.NotNull(player);
        yield return new WaitForSeconds(1f);

        dungeonMaster.DungeonCleared();

        player = GameObject.FindGameObjectWithTag("Player");
        Assert.That(dungeonMaster.Floor == 0);
        Assert.That(dungeonMaster.State == DungeonMaster.DungeonMasterState.DUNGEON_CLEARED);
        Assert.That(dungeonMaster.IsReady() == false);
        Assert.NotNull(player);

        yield return null;

        GameObject newStartPos = GameObject.FindGameObjectWithTag("PlayerSpawn");
        player = GameObject.FindGameObjectWithTag("Player");

        Assert.That(dungeonMaster.State == DungeonMaster.DungeonMasterState.ENTER_DUNGEON);
        Assert.That(dungeonMaster.Floor == 1);
        Assert.That(dungeonMaster.IsReady() == false);

        Assert.NotNull(newStartPos);
        Assert.NotNull(player);

        yield return new WaitForSeconds(1f);

        Assert.That(dungeonMaster.State == DungeonMaster.DungeonMasterState.RUNNING);
        Assert.That(dungeonMaster.IsReady() == true);
        Assert.That(dungeonMaster.Floor == 1);
        Assert.That(newStartPos.transform.position, Is.Not.EqualTo(startPosition).Using(Vector3EqualityComparer.Instance));
    }

    [UnityTest]
    public IEnumerator ShouldLoadWinSceneIfConditionsMet()
    {
        TestSetUp();

        yield return new WaitForSeconds(1f);

        dungeonMaster.DungeonCleared();

        Assert.That(dungeonMaster.State == DungeonMaster.DungeonMasterState.TOWER_BEATEN);
        Assert.That(dungeonMaster.IsReady() == false);

        yield return new WaitForSeconds(1f);

        Assert.That(SceneManager.GetActiveScene().name == "GameWon");
    }


    [UnityTest]
    public IEnumerator ShouldPause()
    {
        dungeonMaster = GameObject.FindGameObjectWithTag("DungeonMaster").GetComponent<DungeonMaster>();
        dungeonMaster.StartDungeonMaster();
        yield return new WaitForSeconds(1f);

        // TODO: Assert on timer

        dungeonMaster.Pause();

        // TODO: Assert timer hasn't changed

        Assert.Fail(); // TODO: Remove once timer assertions are in
        yield return null;
    }

    [UnityTest]
    public IEnumerator ShouldModifyEnemiesStrength()
    {
        yield return null;
        Assert.Fail();
    }

    [UnityTest]
    public IEnumerator ShouldUseGameStateToModifyGame()
    {
        yield return null;
        Assert.Fail();
    }

    [UnityTest]
    public IEnumerator ShouldPassParametersToDungeonGenerator()
    {
        yield return null;
        Assert.Fail();
    }

    [UnityTest]
    public IEnumerator ShouldNotModifyLockedParameters()
    {
        yield return null;
        Assert.Fail();
    }

    [UnityTest]
    public IEnumerator ShouldModifyUnlockedParameters()
    {
        yield return null;
        Assert.Fail();
    }

    private void TestSetUp()
    {
        dungeonMaster = GameObject.FindGameObjectWithTag("DungeonMaster").GetComponent<DungeonMaster>();
        Assert.That(dungeonMaster.State == DungeonMaster.DungeonMasterState.AWAITING_START);
        dungeonMaster.StartDungeonMaster();
    }
}
