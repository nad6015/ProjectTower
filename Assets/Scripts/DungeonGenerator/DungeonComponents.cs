using Unity.AI.Navigation;
using UnityEngine;

namespace Assets.DungeonGenerator
{
    [CreateAssetMenu(fileName = "DungeonComponents", menuName = "Dungeon Components", order = 1)]
    public class DungeonComponents : ScriptableObject
    {
        public GameObject wallTile;
        public GameObject corridorTile;
        public GameObject floorTile;
        public GameObject enemy;
        public GameObject item;
        public SpawnPoint startingPoint;
        public DungeonExit exit;
        public NavMeshSurface navMesh;
    }
}