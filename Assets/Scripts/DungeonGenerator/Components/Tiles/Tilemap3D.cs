using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.DungeonGenerator.Components.Tiles
{
    [CreateAssetMenu(fileName = "Tilemap3D", menuName = "Scriptable Objects/Tilemap3D")]
    public class Tilemap3D : ScriptableObject
    {
        public List<GameObject> wallTiles;
        public List<GameObject> floorTiles;
        public List<GameObject> propTiles;
        public GameObject roomCorner;
        public GameObject corridorArch;

        private Shufflebag<GameObject> _floors;

        public void OnEnable()
        {
            _floors = new(floorTiles);
        }

        private Quaternion rotateY = Quaternion.AngleAxis(-90f, Vector3.up);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <param name="transform"></param>
        /// <returns></returns>
        public GameObject DrawHorizontalWall(float x, float z, Transform transform)
        {
            return Draw(x, z, wallTiles[0], Quaternion.identity, transform);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <param name="quaternion"></param>
        /// <param name="transform"></param>
        /// <returns></returns>
        public GameObject DrawCorridorArch(float x, float z, bool isHorizontal, Transform transform)
        {
            return Draw(x, z, corridorArch, isHorizontal ? rotateY : Quaternion.identity, transform);
        }

        /// <summary>
        /// Draws a floor tile at the given position.
        /// </summary>
        /// <param name="x">the x coordinate</param>
        /// <param name="y">the y coordinate</param>
        /// <param name="transform">the parent transform of this tile</param>
        /// <returns></returns>
        public GameObject DrawFloor(float x, float z, Transform transform)
        {
            return Draw(x, z, _floors.TakeItem(), Quaternion.identity, transform);
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
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <param name="quaternion"></param>
        /// <param name="transform"></param>
        /// <returns></returns>
        public GameObject DrawVerticalWall(float x, float z, Transform transform)
        {
            return Draw(x, z, wallTiles[0], rotateY, transform);
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
            if (_shuffleBag.Count == 0)
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