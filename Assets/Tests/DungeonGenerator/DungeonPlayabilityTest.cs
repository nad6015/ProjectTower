using System.Collections;
using Assets.DungeonGenerator.Components;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using static TestUtilities;
using static Assets.Utilities.GameObjectUtilities;

public class DungeonPlayabilityTest
{
    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("Scenes/Tests/DungeonPlayability");
    }

    [UnityTest]
    [Repeat(10)]
    [Timeout(3600000)]
    public IEnumerator ShouldReachDungeonEnd()
    {
        yield return new WaitForSeconds(1f);
        NavMeshAgent testAgent = FindComponentByTag<NavMeshAgent>("Player");
        Transform endPoint = GameObject.FindFirstObjectByType<DungeonExit>().transform;
        testAgent.SetDestination(endPoint.position);

        do
        {
            yield return new WaitForSeconds(1);
        }

        while (!HasReachedDestination(testAgent.transform, endPoint));
        Assert.That(HasReachedDestination(testAgent.transform, endPoint));
    }
}
