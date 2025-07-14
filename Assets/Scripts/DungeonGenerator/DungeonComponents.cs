using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.DungeonGenerator
{
    public class DungeonComponents : MonoBehaviour
    {
        public GameObject wallAsset;
        public GameObject corridorAsset;
        public GameObject floorAsset;
        public GameObject enemy;
        public GameObject item;
        public SpawnPoint startingPoint;
        public DungeonExit exit;
        public NavMeshSurface navMesh;
    }
}