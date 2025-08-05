using UnityEngine;

namespace Assets.DungeonGenerator
{
    using Random = UnityEngine.Random;
    public class PointUtils
    {
        /// <summary>
        /// Gets a random size between the min and max sizes.
        /// </summary>
        /// <param name="minSize">min size</param>
        /// <param name="maxSize">max size</param>
        /// <returns>a random size</returns>
        public static Vector3 RandomSize(Vector3 minSize, Vector3 maxSize)
        {
            float width = Random.Range(minSize.x, maxSize.x);
            float height = Random.Range(minSize.z, maxSize.z);

            return new Vector3(width, 0, height);
        }

        internal static Vector3 Vec2ToVec3(Vector3 vector, float y)
        {
            return new Vector3(vector.x, y, vector.z);
        }

        internal static Vector3 Vec2ToVec3(Vector2 vector)
        {
            return new Vector3(vector.x, 1, vector.y);
        }

        /// <summary>
        /// Returns a point between two points. Ignores the y component of the Vector3.
        /// </summary>
        /// <param name="v1">min point</param>
        /// <param name="v2">max point</param>
        /// <returns>A random point within the range</returns>
        public static Vector3 RandomPointWithinRange(Vector3 v1, Vector3 v2)
        {
            float x = Random.Range(v1.x, v2.x);
            float z = Random.Range(v1.z, v2.z);
            return new(x, 0, z);
        }

        /// <summary>
        /// Returns a point within the given bounds. Ignores the y component of the Vector3.
        /// </summary>
        /// <param name="v1">min point</param>
        /// <param name="v2">max point</param>
        /// <returns>A random point within the range</returns>
        public static Vector3 RandomPointWithinRange(Bounds b)
        {
            float x = Random.Range(b.min.x, b.max.x);
            float z = Random.Range(b.min.z, b.max.z);
            return new(x, 0, z);
        }
    }
}
