using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DungeonGenerator.Components.Tiles
{
    public class DungeonTile : MonoBehaviour
    {
        [SerializeField]
        public List<AreaType> PlacementAreas;

        [SerializeField]
        public List<RoomType> SuitableRooms;

        public Bounds Bounds { get; private set; }
        private List<GameObject> contents;

        private void Awake()
        {
            contents = new List<GameObject>();
            contents.AddRange(GetChildren(transform));
        }

        private void Start()
        {
            Debug.Log(contents.Count);
            contents = new List<GameObject>();
            contents.AddRange(GetChildren(transform));
        }

        public Bounds GetBounds()
        {
            Bounds bounds = new Bounds();
            // Get game object bounds code referenced from - https://discussions.unity.com/t/getting-the-bounds-of-the-group-of-objects/431270/6
            foreach (Transform child in transform)
            {
                bounds.Encapsulate(child.GetComponentInChildren<Renderer>().bounds);
            }

            return bounds;
        }

        internal bool Contains(GameObject gameObject)
        {
            contents = new List<GameObject>();
            contents.AddRange(GetChildren(transform));
            return contents.Contains(gameObject) && contents.Contains(gameObject.transform.parent.gameObject);
        }

        List<GameObject> GetChildren(Transform transform)
        {
            List<GameObject> children = new List<GameObject>();
            foreach (Transform child in transform)
            {
                children.Add(child.gameObject);
                if (child.childCount > 0)
                {
                    children.AddRange(GetChildren(child));
                }
            }
            return children;
        }
    }
}