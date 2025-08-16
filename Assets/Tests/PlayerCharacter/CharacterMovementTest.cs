using Assets.Combat;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class CharacterMovementTest : InputTestFixture
{
    readonly GameObject player = Resources.Load<GameObject>("Player");
    GameObject character;
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
    public IEnumerator ShouldMovePlayerCharacterWhenKeyIsPressed()
    {
        character = GameObject.Instantiate(player, new Vector3(0, 1), Quaternion.identity);

        float distance = Vector3.Distance(character.transform.position, new Vector3(-2, 0, 1.5f));
        Assert.That(distance, Is.GreaterThan(1));

        Press(keyboard.wKey);
        Press(keyboard.aKey);
        yield return new WaitForSeconds(1f);
        Release(keyboard.wKey);
        Release(keyboard.aKey);
        yield return new WaitForSeconds(1f);

        Assert.That(distance, Is.LessThanOrEqualTo(1));

        Press(keyboard.sKey);
        Press(keyboard.dKey);
        yield return new WaitForSeconds(1.5f);
        Release(keyboard.sKey);
        Release(keyboard.dKey);
        yield return new WaitForSeconds(1.5f);

        distance = Vector3.Distance(character.transform.position, new Vector3(2, 0, -1.5f));
        Assert.That(distance, Is.LessThanOrEqualTo(1));
    }

    [UnityTest]
    public IEnumerator ShouldBeAbleToAttackEnemy()
    {
        Fighter enemy = GameObject.Find("Enemy").GetComponent<Fighter>();
        Vector3 enemyPosition = new(enemy.transform.position.x, 1, enemy.transform.position.z - 1);
        
        character = GameObject.Instantiate(player, enemyPosition, Quaternion.LookRotation(enemyPosition));

        Assert.That(enemy.GetStat(FighterStats.Health), Is.EqualTo(5));
        Press(mouse.leftButton);
        yield return new WaitForSeconds(1f);
        Release(mouse.leftButton);


        Assert.That(enemy.GetStat(FighterStats.Health), Is.EqualTo(4));
    }
}
