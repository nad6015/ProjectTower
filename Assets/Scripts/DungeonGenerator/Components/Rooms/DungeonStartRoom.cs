using Assets.DungeonGenerator.Components.Tiles;
using UnityEngine;

namespace Assets.DungeonGenerator.Components
{
    public class DungeonStartRoom : DungeonRoom
    {
        internal override void Populate(DungeonRepresentation dungeon)
        {
            SpawnPoint startingPoint = dungeon.Components.startingPoint;
            GameObject.Instantiate(startingPoint, Bounds.center, Quaternion.identity, transform);
            DungeonTile tile = startingPoint.GetComponent<DungeonTile>();
            Bounds bounds = tile.GetBounds();

            var hits = Physics.BoxCastAll(Bounds.center, bounds.extents / 2, Vector3.down);
            foreach (var hit in hits)
            {
                if (!tile.Contains(hit.collider.gameObject))
                {
                    hit.collider.gameObject.SetActive(false);
                }
            }
        }

        internal override void InstaniateContents(DungeonRepresentation dungeon)
        {
            // NO-OP, Instantiation happens in Populate
        }
    }
}