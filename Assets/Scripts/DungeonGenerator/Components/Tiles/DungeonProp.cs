using System;
using UnityEngine;

namespace Assets.Scripts.DungeonGenerator.Components.Tiles
{
    [CreateAssetMenu(fileName = "DungeonTile", menuName = "Scriptable Objects/DungeonTile")]
    public class DungeonProp : ScriptableObject
    {
        public GameObject Tile;
        public AreaType Type;
    }
}