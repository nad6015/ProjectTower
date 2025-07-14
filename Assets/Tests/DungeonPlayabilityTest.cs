using System.Collections;
using Assets.DungeonGenerator;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class DungeonPlayability
{
    private int testTimeInSeconds;
    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("Scenes/Tests/DungeonGenerator");
        testTimeInSeconds = 60;
    }

    [UnityTest]
    [Repeat(10)] // TODO: Increase to 500
    public IEnumerator ShouldReachDungeonEnd()
    {
        GameObject.Find("DungeonMaster").GetComponent<DungeonMaster>().OnNewDungeon();
        NavMeshAgent testAgent = GameObject.FindGameObjectWithTag("Player").transform.parent.GetComponent<NavMeshAgent>();

        do
        {
            yield return new WaitForSeconds(1);
            testTimeInSeconds--;
        }
        while (hasReachedDestination(testAgent) && testTimeInSeconds > 0);
        Assert.That(hasReachedDestination(testAgent));
    }

    private bool hasReachedDestination(NavMeshAgent agent)
    {
        // destination condition code referenced from - https://discussions.unity.com/t/how-can-i-tell-when-a-navmeshagent-has-reached-its-destination/52403
        return agent.hasPath || agent.velocity.sqrMagnitude == 0f;
    }
}
