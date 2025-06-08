using Assets.DungeonGenerator.Components;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DungeonGenerator
{
    public class DungeonConstructor : MonoBehaviour
    {
        [SerializeField]
        internal GameObject floorAsset;

        [SerializeField]
        internal GameObject wallAsset;

        readonly Dictionary<string, GameObject> components;

        public DungeonConstructor()
        {
            components = new Dictionary<string, GameObject>();
        }

        internal void PlaceFloor(Dungeon dungeon)
        {
            Debug.Log(floorAsset.name);
            GameObject floor = Instantiate(floorAsset);
            components.Add("floor", floor);
            floor.transform.localScale = new Vector3(dungeon.Size.x, 0, dungeon.Size.y);
            floor.transform.localPosition = Vector3.zero;
        }

        internal void PlaceWall(DungeonRoom room)
        {
           
        }

        internal void PlaceContent(Dungeon dungeon)
        {

        }
    }
}