
using Assets.DungeonGenerator.Components;
using System;
using UnityEngine;

namespace Assets.DungeonGenerator
{
    public class DungeonRoom : MonoBehaviour
    {
        private static int roomId = 0;
        public Rect Bounds { get; set; }

        internal DungeonRoom(Rect bounds)
        {
            Bounds = bounds;
        }

        public void ConstructRoom(GameObject floorAsset, GameObject wallAsset)
        {
            Debug.Log(floorAsset.name);
            // Place the floor
            GameObject floor = Instantiate(floorAsset);
            floor.transform.SetParent(transform);
            floor.transform.localScale = Vec2ToVec3(Bounds.size, 0.5f);
            floor.transform.localPosition = Vec2ToVec3(Bounds.position, 0);

      
            //parentRoom.transform.localPosition = Vec2ToVec3(room.Bounds.position);

            // Place the top wall
            for (int i = 0; i < Bounds.width; i++)
            {
                GameObject wall = Instantiate(wallAsset);
                wall.transform.localPosition = new Vector3(Bounds.position.x + i, 0, Bounds.position.y);
                wall.name = "Wall cap";
                wall.transform.SetParent(transform);
            }

            // Place left and right walls
            for (int i = 0; i < Bounds.height; i++)
            {
                GameObject wall = Instantiate(wallAsset);
                wall.name = "Wall length";
                wall.transform.localPosition = new Vector3(Bounds.position.x, 0, i + Bounds.y);
                wall.transform.SetParent(transform);

                GameObject wall2 = Instantiate(wallAsset);
                wall2.name = "Wall length";
                wall2.transform.localPosition = new Vector3(Bounds.width + Bounds.position.x, 0, i + Bounds.y);
                wall2.transform.SetParent(transform);
            }

            // Place bottom wall
            for (int i = 0; i < Bounds.width; i++)
            {
                GameObject wall = Instantiate(wallAsset);
                wall.transform.localPosition = new Vector3(Bounds.position.x + i, 0, Bounds.height + Bounds.position.y);
                wall.name = "Wall cap";
                wall.transform.SetParent(transform);
            }
        }

        private Vector3 Vec2ToVec3(Vector2 vector, float y)
        {
            return new Vector3(vector.x, y, vector.y);
        }

        internal static DungeonRoom Create(Rect bounds)
        {
            // Create gameobject code referenced from  - https://discussions.unity.com/t/how-do-you-create-an-empty-gameobject-in-code-and-add-it-to-the-scene/86380/4
            GameObject gameObj = new("Room " + roomId++);
            DungeonRoom dungeonRoom = gameObj.AddComponent<DungeonRoom>();
            dungeonRoom.Bounds = bounds;

            return dungeonRoom;
        }
    }
}