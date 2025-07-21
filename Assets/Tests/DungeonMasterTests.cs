using System.Collections;
using Assets.DungeonGenerator;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

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
        Assert.That(dungeonMaster.state == DungeonMaster.State.AWAITING_START);
        
        dungeonMaster.StartDungeonMaster();

        Assert.That(dungeonMaster.state == DungeonMaster.State.STARTING);

        yield return new WaitForSeconds(1f);

        Assert.That(dungeonMaster.state == DungeonMaster.State.MONITORING);
        Assert.That(dungeonMaster.IsReady());

        yield return null;
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
}
