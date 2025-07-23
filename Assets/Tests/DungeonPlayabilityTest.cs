using System.Collections;
using Assets.DungeonGenerator;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class DungeonPlayability
{
    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("Scenes/Tests/DungeonGenerator");
    }

    [UnityTest]
    [Repeat(10)] // TODO: Increase to 500
    public IEnumerator ShouldReachDungeonEnd()
    {
        GameObject.Find("DungeonMaster").GetComponent<DungeonMaster>().NewDungeon();
        NavMeshAgent testAgent = GameObject.FindGameObjectWithTag("Player").transform.parent.GetComponent<NavMeshAgent>();
        Transform endPoint = GameObject.FindFirstObjectByType<DungeonExit>().transform;

        do
        {
            yield return new WaitForSeconds(1);
        }

        while (!HasReachedDestination(testAgent.transform, endPoint));
        Assert.That(HasReachedDestination(testAgent.transform, endPoint));
    }

    private bool HasReachedDestination(Transform agent, Transform endpoint)
    {
        return Vector3.Distance(endpoint.position, agent.position) < 1;
    }
}
