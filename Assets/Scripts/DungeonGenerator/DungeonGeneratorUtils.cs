using UnityEngine;

namespace Assets.DungeonGenerator
{
    using Random = UnityEngine.Random;
    internal class DungeonGeneratorUtils
    {
        internal static Vector2 GenerateRandomSize(Vector2 minSize, Vector2 maxSize)
        {
            float width = Random.Range(minSize.x, maxSize.x);
            float height = Random.Range(minSize.y, maxSize.y);

            return new Vector2(width, height);
        }

        internal static Vector3 Vec2ToVec3(Vector2 vector, float y)
        {
            return new Vector3(vector.x, y, vector.y);
        }

        internal static Vector3 Vec2ToVec3(Vector2 vector)
        {
            return new Vector3(vector.x, 1, vector.y);
        }

        internal static Vector3 GetRandomPointWithinBounds(Rect bounds)
        {
            float x = Random.Range(bounds.x + 1, bounds.xMax - 1);
            float y = Random.Range(bounds.y + 1, bounds.yMax - 1);
            return new(x, 0, y);
        }
    }
}
