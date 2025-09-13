using System.Collections;
using Assets.DungeonGenerator.Components;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using static Assets.Utilities.GameObjectUtilities;
using static TestUtilities;
public class DungeonComponentsTest : InputTestFixture
{
    Keyboard keyboard;
    Mouse mouse;

    [SetUp]
    public override void Setup()
    {
        SceneManager.LoadScene("Scenes/Tests/DungeonComponents");
        base.Setup();
        keyboard = InputSystem.AddDevice<Keyboard>();
        mouse = InputSystem.AddDevice<Mouse>();

        Press(mouse.rightButton);
        Release(mouse.rightButton);
    }

    [UnityTest]
    public IEnumerator ShouldLeaveDungeonViaDungeonExit()
    {
        yield return new WaitForSeconds(1f);
        bool leftDungeon = false;
        NavMeshAgent testAgent = FindComponentByTag<NavMeshAgent>("Player");
        DungeonExit exit = GameObject.FindFirstObjectByType<DungeonExit>();
        Transform exitTransform = exit.transform;

        exit.DungeonCleared += () => leftDungeon = true;
        testAgent.SetDestination(exitTransform.position);
        do
        {
            yield return new WaitForSeconds(1);
        }
        while (!HasReachedDestination(testAgent.transform, exitTransform));
        Assert.That(HasReachedDestination(testAgent.transform, exitTransform));
        Press(keyboard.eKey);
        yield return new WaitForSeconds(1);

        Assert.That(leftDungeon, Is.True);
    }
}
