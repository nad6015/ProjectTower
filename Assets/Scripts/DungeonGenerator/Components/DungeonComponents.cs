using Assets.DungeonGenerator.Components.Tiles;
using Assets.Interactables;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DungeonGenerator
{
    [CreateAssetMenu(fileName = "DungeonComponents", menuName = "Dungeon Components", order = 1)]
    public class DungeonComponents : ScriptableObject
    {
        public DungeonTilemap tilemap;

        public List<GameObject> enemies;
        public SpawnPoint startingPoint;
        public DungeonExit exit;
        public PickupItem doorKey;
    }
}