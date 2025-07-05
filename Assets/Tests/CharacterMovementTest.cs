using Assets.CombatSystem;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.TestTools.Utils;

public class CharacterMovementTest : InputTestFixture
{
    GameObject player = Resources.Load<GameObject>("Player");
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
        Press(keyboard.wKey);
        Press(keyboard.aKey);
        yield return new WaitForSeconds(1f);
        Release(keyboard.wKey);
        Release(keyboard.aKey);
        yield return new WaitForSeconds(1f);

        float distance = Vector3.Distance(character.transform.position, new Vector3(-2, 0, 1.5f));
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
        character = GameObject.Instantiate(player, new Vector3(0, 1), Quaternion.identity);

        Fighter enemy = GameObject.Find("Enemy").GetComponent<Fighter>();
        Vector3 enemyPosition = new Vector3(enemy.transform.position.x, character.transform.position.y, enemy.transform.position.z - 1);

        character.transform.position = enemyPosition;

        Assert.That(enemy.GetStat(FighterStats.HEALTH), Is.EqualTo(5));
        Press(mouse.leftButton);
        yield return new WaitForSeconds(1f);

        Assert.That(enemy.GetStat(FighterStats.HEALTH), Is.EqualTo(4));
    }
}
