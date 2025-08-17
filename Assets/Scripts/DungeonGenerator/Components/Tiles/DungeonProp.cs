using UnityEngine;

namespace Assets.DungeonGenerator.Components.Tiles
{
    public class DungeonProp : MonoBehaviour
    {
        [SerializeField]
        public AreaType Type;

        [SerializeField]
        public Vector2 TileSize = new(1, 1);
    }
}