using Assets.DungeonGenerator.Components.Tiles;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DungeonGenerator
{
    [CreateAssetMenu(fileName = "DungeonComponents", menuName = "Dungeon Components", order = 1)]
    public class DungeonComponents : ScriptableObject
    {
        public GameObject wallTile;
        public GameObject floorTile;
        public Tilemap3D tilemap;

        public GameObject corridorWall;
        public GameObject corridorFloor;


        public List<GameObject> enemies;
        public List<GameObject> roomContent;
        public SpawnPoint startingPoint;
        public DungeonExit exit;
     
    }
}