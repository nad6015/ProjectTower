
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.DungeonGenerator
{
    public class DungeonCorridor : MonoBehaviour
    {
        static int corridorId = 0;
        internal Rect Bounds { get; set; }
        internal DungeonAxis Axis { get { return axis; }}

        private DungeonAxis axis;
        private RaycastHit hit;
        public DungeonCorridor(Rect bounds)
        {
            Bounds = bounds;
        }

        // Place corridor
        internal void Construct(GameObject corridorAsset, GameObject floorAsset)
        {
            // Don[t place anything if corridor has no heightor width. This means the rooms share a boundary
            if(Mathf.Approximately(Bounds.width,0)|| Mathf.Approximately(Bounds.height, 0))
            {
                transform.position = new(Bounds.x, 0, Bounds.y);
                return;
            }

            GameObject floor = Instantiate(floorAsset);
            floor.transform.SetParent(transform);
            floor.transform.localScale = DungeonGeneratorUtils.Vec2ToVec3(Bounds.size, 0.5f);
            floor.transform.localPosition = DungeonGeneratorUtils.Vec2ToVec3(Bounds.position, 0);

            int count = Mathf.RoundToInt(axis == DungeonAxis.VERTICAL ? Bounds.height : Bounds.width);

            for (int i = 0; i < count; i++)
            {
                float wallX = axis == DungeonAxis.VERTICAL ? Bounds.position.x : Bounds.position.x + i;
                float wallY = axis == DungeonAxis.VERTICAL ? Bounds.position.y + i : Bounds.position.y;

                //if(Physics.Raycast(new Vector3(wallX+.5f, 2f, wallY-0.5f), Vector3.down, out hit))
                //{
                //    GameObject.Destroy(hit.collider.gameObject);
                //}

                GameObject corridor = Instantiate(corridorAsset);
                corridor.transform.localPosition = new Vector3(wallX, 0, wallY);
                corridor.name = "Corridor";
                corridor.transform.SetParent(transform);

                wallX = axis == DungeonAxis.VERTICAL ? Bounds.position.x + Bounds.size.x : Bounds.position.x + i;
                wallY = axis == DungeonAxis.VERTICAL ? Bounds.position.y + i : Bounds.position.y + Bounds.size.y;

                //if (Physics.Raycast(new Vector3(wallX, 2f, wallY), Vector3.down, out hit))
                //{
                //    GameObject.Destroy(hit.collider.gameObject);
                //}

                GameObject corridor2 = Instantiate(corridorAsset);
                corridor2.transform.localPosition = new Vector3(wallX, 0, wallY);
                corridor2.name = "Corridor";
                corridor2.transform.SetParent(transform);
            }
        }

        internal static DungeonCorridor Create(Rect bounds, DungeonAxis axis)
        {
            // Create gameobject code referenced from  - https://discussions.unity.com/t/how-do-you-create-an-empty-gameobject-in-code-and-add-it-to-the-scene/86380/4
            GameObject gameObj = new("Corridor " + corridorId++);
            DungeonCorridor corridor = gameObj.AddComponent<DungeonCorridor>();
            corridor.Bounds = bounds;
            corridor.axis = axis;
            return corridor;
        }
    }
}