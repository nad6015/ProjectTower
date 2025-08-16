using Assets.DungeonGenerator.Components.Tiles;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DungeonGenerator.Components
{
    public static class DungeonComponentUtils
    {
        public static List<GameObject> DrawFloor(Tilemap3D tilemap, BoundsInt bounds, Transform transform)
        {
            int width = bounds.size.x;
            int height = bounds.size.z;

            return tilemap.DrawFloor(bounds.min, width, height, transform);
        }

        public static IEnumerable<GameObject> DrawTopAndBottomWalls(Tilemap3D tilemap, BoundsInt bounds, Transform transform)
        {
            int width = bounds.size.x;

            int minX = bounds.min.x;
            int minZ = bounds.min.z;

            int maxZ = bounds.max.z;

            return tilemap.DrawHorizontalWalls(width, minX, minZ, maxZ, transform);
        }

        public static IEnumerable<GameObject> DrawLeftAndRightWalls(Tilemap3D tilemap, BoundsInt bounds, Transform transform)
        {
            int height = bounds.size.z;

            int minX = bounds.min.x;
            int minZ = bounds.min.z;

            int maxX = bounds.max.x;

            return tilemap.DrawVerticalWalls(height, minX, maxX, minZ, transform);
        }

        public static BoundsInt BoundsToBoundsInt(Bounds bounds)
        {
            return new BoundsInt(
                Mathf.FloorToInt(bounds.min.x),
                0,
                Mathf.FloorToInt(bounds.min.z),
                Mathf.FloorToInt(bounds.size.x),
                0,
                Mathf.FloorToInt(bounds.size.z));
        }
    }
}