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
        private Shufflebag<GameObject> _setPieces;

        public void OnEnable()
        {
            _floors = new(floorTiles);
            _setPieces = new(propTiles);
        }

        private Quaternion rotateY = Quaternion.AngleAxis(-90f, Vector3.up);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <param name="transform"></param>
        /// <returns></returns>
        public IEnumerable<GameObject> DrawHorizontalWalls(int width, int minX, int z, int maxZ, Transform transform)
        {
            List<GameObject> walls = new();
            for (int i = 0; i < width; i++)
            {
                float wallX = minX + i;

                // Bottom wall
                walls.Add(Draw(wallX, z, wallTiles[0], Quaternion.identity, transform));

                // Top wall
                walls.Add(Draw(wallX, maxZ, wallTiles[0], Quaternion.identity, transform));
            }
            return walls;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="x"></param>
        /// <param name="minZ"></param>
        /// <param name="quaternion"></param>
        /// <param name="transform"></param>
        /// <returns></returns>
        public List<GameObject> DrawVerticalWalls(int height, int x, int maxX, int minZ, Transform transform)
        {
            List<GameObject> walls = new();
            for (int i = 0; i < height; i++)
            {
                float z = minZ + i;

                // Left wall
                walls.Add(Draw(x, z, wallTiles[0], rotateY, transform));

                // Right wall
                walls.Add(Draw(maxX, z, wallTiles[0], rotateY, transform));
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

        internal DungeonProp GetProp()
        {
            return _setPieces.TakeItem()?.GetComponent<DungeonProp>();
        }
    }

    public class Shufflebag<T>
    {
        private List<T> _originalList;
        private List<T> _shuffleBag;
        public Shufflebag(List<T> originalList)
        {
            _originalList = new List<T>(originalList);
            _shuffleBag = new List<T>(originalList);
        }

        public T TakeItem()
        {
            if (_shuffleBag.Count == 0 && _originalList.Count == 0)
            {
                return default;
            }
            else if (_shuffleBag.Count == 0)
            {
                _shuffleBag = new List<T>(_originalList);
            }

            int randomIndex = Mathf.RoundToInt(UnityEngine.Random.value * (_shuffleBag.Count - 1));

            T item = _shuffleBag[randomIndex];
            _shuffleBag.RemoveAt(randomIndex);

            return item;
        }
    }
}