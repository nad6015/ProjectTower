using Assets.Combat;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using static Assets.Utilities.GameObjectUtilities;

public class CharacterMovementTest : InputTestFixture
{
    Transform player;
    Keyboard keyboard;
    Mouse mouse;

    public override void Setup()
    {
        SceneManager.LoadScene("Scenes/Tests/PlayerMechanics");
        base.Setup();
        keyboard = InputSystem.AddDevice<Keyboard>();
        mouse = InputSystem.AddDevice<Mouse>();

        Press(mouse.rightButton);
        Release(mouse.rightButton);
    }

    [UnityTest]
    public IEnumerator ShouldMovePlayerCharacterWhenKeysArePressed()
    {
        player = FindComponentByTag<Transform>("Player");
        Vector3 waKeysPos = new(-2.5f, 0, 2.5f);
        Vector3 sdKeysPos = new(2.5f, 0, -2.5f);

        float distance = Vector3.Distance(player.position, waKeysPos);
        Assert.That(distance, Is.GreaterThan(1));

        Press(keyboard.wKey);
        Press(keyboard.aKey);
        yield return new WaitForSeconds(0.75f);
        Release(keyboard.wKey);
        Release(keyboard.aKey);
        yield return new WaitForSeconds(1f);
       
        distance = Vector3.Distance(player.position, waKeysPos);

        Assert.That(distance, Is.LessThanOrEqualTo(1));;
   

        distance = Vector3.Distance(player.position, sdKeysPos);
        Assert.That(distance, Is.GreaterThan(1));

        Press(keyboard.sKey);
        Press(keyboard.dKey);
        yield return new WaitForSeconds(1.5f);
        Release(keyboard.sKey);
        Release(keyboard.dKey);
        yield return new WaitForSeconds(1f);

        distance = Vector3.Distance(player.position, sdKeysPos);
        Assert.That(distance, Is.LessThanOrEqualTo(1));
    }

    [UnityTest]
    public IEnumerator ShouldBeAbleToAttackEnemy()
    {
        Fighter enemy = GameObject.Find("Dummy").GetComponent<Fighter>();
        player = FindComponentByTag<Transform>("Player");
        Vector3 enemyPosition = new(player.transform.position.x, 1, player.transform.position.z + 1);
        enemy.transform.SetPositionAndRotation(enemyPosition, Quaternion.identity);

        Assert.That(enemy.GetStat(FighterStats.Health), Is.EqualTo(5));
        Press(mouse.leftButton);
        yield return new WaitForSeconds(1f);
        Release(mouse.leftButton);


        Assert.That(enemy.GetStat(FighterStats.Health), Is.EqualTo(4));
    }
}
