using System.Collections;
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
using Assets.Interactables;
using Assets.PlayerCharacter;

public class GameplayAdaptionTests
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
    public IEnumerator ShouldMonitorEnemiesDefeatedAndIncreaseTreasureChestDropAccordingly()
    {
        TestSetUp();

        yield return new WaitForSeconds(1f);

        Assert.That(dungeonMaster.State == DungeonMasterState.Running);

        TreasureChest chest = GameObject.FindFirstObjectByType<TreasureChest>();
        
        Assert.That(chest == null);

        var enemies = GameObject.FindObjectsByType<TestNpcFighter>(FindObjectsSortMode.None);

        // Defeat one enemies to increase treasure chest drop rate by 100
        player.GetComponent<TestPlayableFighter>().DefeatRandomEnemy();  
        
        yield return new WaitForSeconds(1f);

        chest = GameObject.FindFirstObjectByType<TreasureChest>();

        Assert.That(chest != null);
    }

    [UnityTest]
    public IEnumerator ShouldMonitorCharacterHealthAndAdjustHealingItemDropsAccordingly()
    {
        TestSetUp();

        yield return new WaitForSeconds(1f);

        Assert.That(dungeonMaster.State == DungeonMasterState.Running);

        ResourceSystem resourceSystem = FindComponentByTag<ResourceSystem>("ResourceSystem");

        Assert.That(resourceSystem.TakeContainerItem() == null);

        player.GetComponent<TestPlayableFighter>().DamageSelf(4); // Restoration drop rate should increase by 100
        yield return new WaitForSeconds(1f);

        Assert.That(resourceSystem.TakeContainerItem() != null);

        player.GetComponent<TestPlayableFighter>().Heal(5); // Restorative item drop rate should decrease by 100
        yield return new WaitForSeconds(1f);

        Assert.That(resourceSystem.TakeContainerItem() == null);
    }
    private void TestSetUp()
    {
        dungeonMaster = FindComponentByTag<DungeonMaster>("DungeonMaster");
        player = FindGameObjectWithTag("Player");
    }
}
