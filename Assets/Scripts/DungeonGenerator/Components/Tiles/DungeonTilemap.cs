using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.DungeonGenerator.Components.Tiles
{
    [CreateAssetMenu(fileName = "Tilemap3D", menuName = "Dungeon Tilemap")]
    public class DungeonTilemap : ScriptableObject
    {
        public List<GameObject> wallTiles;
        public List<GameObject> floorTiles;
        public List<GameObject> propTiles;
        public GameObject roomCorner;
        public GameObject corridorDoor;
        public Color mainCameraColor;

        public const int TileUnit = 1;

        private Shufflebag<GameObject> _floors;
        private Shufflebag<GameObject> _walls;
        private Shufflebag<GameObject> _setPieces;

        public void OnEnable()
        {
            _floors = new(floorTiles);
            _walls = new(wallTiles);
            _setPieces = new(propTiles);
        }

        private Quaternion rotateY = Quaternion.AngleAxis(-90f, Vector3.up);

        /// <summary>
        /// Draws the left and right walls of a room.
        /// </summary>
        /// <param name="width">the width of the room</param>
        /// <param name="minX">the x component of the room's starting point</param>
        /// <param name="z">the z component of the room's starting</param>
        /// <param name="maxZ">the z component of the room's furthermost point</param>
        /// <param name="transform">the room's transform</param>
        /// <returns>A collection of the created walls</returns>
        public IEnumerable<GameObject> DrawHorizontalWalls(int width, int minX, int z, int maxZ, Transform transform)
        {
            List<GameObject> walls = new();
            for (int i = 0; i < width; i++)
            {
                float wallX = minX + i;

                // Bottom wall
                walls.Add(Draw(wallX, z, _walls.TakeItem(), Quaternion.AngleAxis(180, Vector3.up), transform));

                // Top wall
                walls.Add(Draw(wallX, maxZ, _walls.TakeItem(), Quaternion.identity, transform));
            }
            return walls;
        }

        /// <summary>
        /// Draws the top and bottom walls of a room.
        /// </summary>
        /// <param name="height">the height of the room</param>
        /// <param name="x">the z component of the room's starting</param>
        /// <param name="maxX">the x component of the room's furthermost point</param>
        /// <param name="minZ">the z component of the room's starting point</param>
        /// <param name="transform">the room's transform</param>
        /// <returns>A collection of the created walls</returns>
        public List<GameObject> DrawVerticalWalls(int height, int x, int maxX, int minZ, Transform transform)
        {
            List<GameObject> walls = new();
            for (int i = 0; i < height; i++)
            {
                float z = minZ + i;

                // Left wall
                walls.Add(Draw(x, z, _walls.TakeItem(), rotateY, transform));

                // Right wall
                walls.Add(Draw(maxX, z, _walls.TakeItem(), Quaternion.Inverse(rotateY), transform));
            }
            return walls;
        }

        /// <summary>
        /// Draw the corridor's doors. Hardcoded values are used (the +4 and -1) in this method as the corridor door model is
        /// knwon and unlikely to change.
        /// </summary>
        /// <param name="bounds">the bounds of the corridor</param>
        /// <param name="isHorizontal"></param>
        /// <param name="transform"></param>
        /// <returns></returns>
        public List<GameObject> DrawCorridorDoors(BoundsInt bounds, bool isHorizontal, Transform transform)
        {
            List<GameObject> doors = new();

            if (isHorizontal)
            {
                doors.Add(Draw(bounds.xMin, bounds.zMin + 4, corridorDoor, Quaternion.Inverse(rotateY), transform));
                doors.Add(Draw(bounds.xMax - 1, bounds.zMin, corridorDoor, rotateY, transform));
            }
            else
            {
                doors.Add(Draw(bounds.xMin, bounds.zMin, corridorDoor, Quaternion.identity, transform));
                doors.Add(Draw(bounds.xMin + 4, bounds.zMax - 1, corridorDoor, Quaternion.AngleAxis(180f, Vector3.up), transform));
            }

            return doors;
        }

        /// <summary>
        /// Draws a floor tile at the given position.
        /// </summary>
        /// <param name="x">the x coordinate</param>
        /// <param name="y">the y coordinate</param>
        /// <param name="transform">the parent transform of this tile</param>
        /// <returns></returns>
        public List<GameObject> DrawFloor(Vector3 start, int width, int height, Transform transform)
        {
            List<GameObject> drawnFloorTiles = new();
            int startX = Mathf.FloorToInt(start.x);
            int startZ = Mathf.FloorToInt(start.z);

            for (int i = 0; i < width; i++)
            {
                int x = startX + i;
                for (int j = 0; j < height; j++)
                {
                    int z = startZ + j;
                    drawnFloorTiles.Add(Draw(x, z, _floors.TakeItem(), Quaternion.identity, transform));
                }
            }
            return drawnFloorTiles;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <param name="tile"></param>
        /// <param name="quaternion"></param>
        /// <param name="transform"></param>
        /// <returns></returns>
        private GameObject Draw(float x, float z, GameObject tile, Quaternion quaternion, Transform transform)
        {
            GameObject gameObject = Instantiate(tile, new Vector3(x, 0, z), Quaternion.identity, transform);
            var model = gameObject.transform.GetChild(0);
            model.rotation *= quaternion;
            return gameObject;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="bounds"></param>
        /// <param name="transform"></param>
        public GameObject DrawRoomCorner(float x, float z, Transform transform)
        {
            return Draw(x, z, roomCorner, Quaternion.identity, transform);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="max"></param>
        /// <param name="transform"></param>
        public GameObject DrawRoomCorner(Vector3Int max, Transform transform)
        {
            return DrawRoomCorner(max.x, max.z, transform);
        }

        internal DungeonTile GetProp()
        {
            return _setPieces.TakeItem()?.GetComponent<DungeonTile>();
        }
    }
}