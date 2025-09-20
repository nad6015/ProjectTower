using Assets.Combat;
using Assets.DungeonGenerator.Components;
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
        public Enemy boss;

        public SpawnPoint startingPoint;
        public DungeonExit exit;
        public PickupItem doorKey;
        public TreasureChest chest;

        public AudioClip FootstepAudio;
        public AudioClip DungeonMusic;
    }
}