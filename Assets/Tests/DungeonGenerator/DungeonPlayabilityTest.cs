using System.Collections;
using Assets.DungeonGenerator.Components;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using static TestUtilities;
using static Assets.Utilities.GameObjectUtilities;
using Assets.Combat;

namespace Tests.DungeonPlayablity
{
    public class DungeonPlayabilityTest
    {
        const int testTimeoutMs = (3600000 * 8) + (600000);
        [SetUp]
        public void Setup()
        {
            SceneManager.LoadScene("Scenes/Tests/DungeonPlayability");
        }

        [UnityTest]
        [Repeat(500)]
        [Timeout(testTimeoutMs)]
        public IEnumerator ShouldReachDungeonEnd()
        {
            yield return new WaitForSeconds(1f);
            var enemies = GameObject.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
            foreach (var enemy in enemies)
            {
                enemy.gameObject.SetActive(false);
            }

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
}