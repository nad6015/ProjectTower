using System.Collections;
using Assets.DungeonGenerator;
using Assets.DungeonMaster;
using Assets.PlayerCharacter;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.TestTools.Utils;

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
        
        Assert.That(dungeonMaster.State == DungeonMasterState.RUNNING);

        Assert.NotNull(dungeonGenerator);
        Assert.That(dungeonGenerator.transform.childCount > 15);

        Assert.NotNull(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>());

        yield return null;
    }

    [UnityTest]
    public IEnumerator ShouldGenerateNewDungeonOnClear()
    {
        TestSetUp();

        Assert.That(dungeonMaster.State == DungeonMasterState.RUNNING);
        Assert.That(dungeonMaster.Floor == 1);

        GameObject startPos = GameObject.FindGameObjectWithTag("PlayerSpawn");

        Vector3 startPosition = startPos.transform.position;

        Assert.NotNull(startPos);
        Assert.NotNull(player);

        yield return new WaitForSeconds(1f);

        dungeonMaster.OnDungeonCleared();

        player = GameObject.FindGameObjectWithTag("Player");
        Assert.That(dungeonMaster.Floor == 1);
        Assert.That(dungeonMaster.State == DungeonMasterState.GENERATE_DUNGEON);
        Assert.NotNull(player);

        yield return new WaitForSeconds(1f);

        GameObject newStartPos = GameObject.FindGameObjectWithTag("PlayerSpawn");
        player = GameObject.FindGameObjectWithTag("Player");

        Assert.That(dungeonMaster.State == DungeonMasterState.RUNNING);
        Assert.That(dungeonMaster.Floor == 2);

        Assert.NotNull(newStartPos);
        Assert.NotNull(player);
        Assert.That(newStartPos.transform.position, Is.Not.EqualTo(startPosition).Using(Vector3EqualityComparer.Instance));
    }

    [UnityTest]
    public IEnumerator ShouldLoadLoseSceneIfConditionsMet()
    {
        TestSetUp();

        yield return new WaitForSeconds(1f);
        
        dungeonMaster.OnDungeonCleared();

        Assert.That(dungeonMaster.State == DungeonMasterState.GENERATE_DUNGEON);

        yield return new WaitForSeconds(1f);
        Assert.That(dungeonMaster.State == DungeonMasterState.GAME_END);
    }

    [UnityTest]
    public IEnumerator ShouldUseGameStateToModifyEventChance()
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
        TestSetUp();

        yield return null;
        Assert.Fail();
    }

    private void TestSetUp()
    {
        dungeonMaster = GameObject.FindGameObjectWithTag("DungeonMaster").GetComponent<DungeonMaster>();
        dungeonGenerator = GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<DungeonGenerator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
}
