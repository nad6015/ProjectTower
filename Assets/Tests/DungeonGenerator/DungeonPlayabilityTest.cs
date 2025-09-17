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
using Assets.DungeonGenerator;
using Assets.DungeonMaster;
using Unity.AI.Navigation;

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

            // Remove obstacles from navmesh path
            var enemies = GameObject.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
            foreach (var enemy in enemies)
            {
                enemy.gameObject.SetActive(false);
            }

            var items = GameObject.FindObjectsByType<DestructibleItem>(FindObjectsSortMode.None);
            foreach (var item in items)
            {
                item.gameObject.SetActive(false);
            }

            // Regen navmesh
            FindComponentByTag<DungeonGenerator>("DungeonGenerator").GetComponent<NavMeshSurface>().BuildNavMesh();

            // Get navmesh agent and set its destination
            NavMeshAgent testAgent = FindComponentByTag<NavMeshAgent>("Player");
            Transform endPoint = GameObject.FindFirstObjectByType<DungeonExit>().transform;
            DungeonLayout layout = FindComponentByTag<DungeonMaster>("DungeonMaster").Layout;
            testAgent.SetDestination(endPoint.position);
            Vector3 lastPos = Vector3.zero;
            do
            {
                yield return new WaitForSeconds(2f);
                // If the navmesh agent gets stucks then attempt a reset.
                if (lastPos == testAgent.transform.position)
                {
                    Debug.Log("Navmesh agent got stuck. Attempting to rest its path.");
                    testAgent.transform.position = layout.FirstNode.Bounds.center;
                    endPoint = GameObject.FindFirstObjectByType<DungeonExit>().transform;
                    testAgent.SetDestination(endPoint.position);
                }
                else
                {
                    lastPos = testAgent.transform.position;
                }
            }
            while (!HasReachedDestination(testAgent.transform, endPoint));
            Assert.That(HasReachedDestination(testAgent.transform, endPoint));
        }
    }
}