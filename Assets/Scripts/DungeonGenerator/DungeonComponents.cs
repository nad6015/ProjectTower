using UnityEngine;

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
    }
}