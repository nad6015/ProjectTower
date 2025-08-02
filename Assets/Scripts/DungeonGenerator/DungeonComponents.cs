using Assets.Combat;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

namespace Assets.DungeonGenerator
{
    [CreateAssetMenu(fileName = "DungeonComponents", menuName = "Dungeon Components", order = 1)]
    public class DungeonComponents : ScriptableObject
    {
        public GameObject wallTile;
        public GameObject floorTile;

        public GameObject corridorWall;
        public GameObject corridorFloor;


        public List<GameObject> enemies;
        public List<GameObject> roomContent;
        public SpawnPoint startingPoint;
        public DungeonExit exit;
     
    }
}