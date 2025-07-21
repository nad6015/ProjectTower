using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DungeonGenerator
{
    public class DungeonCorridor : MonoBehaviour
    {
        static int corridorId = 0;
        public List<Tuple<Rect, DungeonAxis>> Paths { get; private set; }
        private List<GameObject> walls = new();


        // Place corridor
        internal void Construct(DungeonComponents components)
        {
            // TODO: Construct should be idempotent or not called more than once
            if (walls.Count > 0)
            {
                return;
            }
            GameObject corridorAsset = components.corridorTile;
            GameObject floorAsset = components.floorTile;

            if (Paths.Count > 1)
            {
                ////ConstructCorridors(corridorAsset, floorAsset);
                ConstructCorridor(corridorAsset, floorAsset, Paths[0].Item1, Paths[0].Item2);
                //Debug.Log("path = " + Paths[0].Item1 + ", axis = " + Paths[0].Item2);
                ConstructCorridor(corridorAsset, floorAsset, Paths[1].Item1, Paths[1].Item2);
                //Debug.Log("path1 = " + Paths[1].Item1 + ", axis = " + Paths[1].Item2);

            }
            else if (Paths.Count == 1)
            {
                ConstructCorridor(corridorAsset, floorAsset, Paths[0].Item1, Paths[0].Item2);
            }
        }

        private void ConstructCorridors(GameObject wallAsset, GameObject floorAsset)
        {
            Rect path = Paths[0].Item1;
            Rect path1 = Paths[1].Item1;

            DungeonAxis axis = Paths[0].Item2;
            DungeonAxis axis1 = Paths[1].Item2;


            // Don't place anything if corridor has already been built. TODO: This might be hack
            if (transform.childCount > 0)
            {
                return;
            }

            GameObject floor = Instantiate(floorAsset);
            floor.transform.SetParent(transform);
            floor.transform.localScale = DungeonGeneratorUtils.Vec2ToVec3(path.size, 0.5f);
            floor.transform.localPosition = DungeonGeneratorUtils.Vec2ToVec3(path.position, 0);

            floor = Instantiate(floorAsset);
            floor.transform.SetParent(transform);
            floor.transform.localScale = DungeonGeneratorUtils.Vec2ToVec3(path1.size, 0.5f);
            floor.transform.localPosition = DungeonGeneratorUtils.Vec2ToVec3(path1.position, 0);

            float x = Mathf.Min(path1.x, path.x);
            float y = Mathf.Min(path1.y, path.y);

            // Place the bottom wall
            for (int i = 0; i < path.width + path1.width; i++)
            {
                float wallX = x + i;
                float wallY = y;

                GameObject wall = Instantiate(wallAsset);
                wall.transform.localPosition = new Vector3(wallX, 0, wallY);
                wall.name = "Wall cap";
                wall.transform.SetParent(transform);
                walls.Add(wall);
            }

            // Place left and right walls
            for (int i = 0; i < path.height + path1.height; i++)
            {
                float wallX = x + i;
                float wallX2 = (path1.width + path.width) + wallX;
                float wallY = i + path.y;

                GameObject wall = Instantiate(wallAsset);
                wall.name = "Wall length";
                wall.transform.localPosition = new Vector3(wallX, 0, wallY);
                wall.transform.SetParent(transform);
                walls.Add(wall);

                GameObject wall2 = Instantiate(wallAsset);
                wall2.name = "Wall length";
                wall2.transform.localPosition = new Vector3(wallX2, 0, wallY);
                wall2.transform.SetParent(transform);
                walls.Add(wall2);
            }

            // Place top wall
            for (int i = 0; i < path.width + path1.width; i++)
            {
                float wallX = (path1.width + path.width) + x + i;
                float wallY = Mathf.Min(path1.height, path.height) + y;

                GameObject wall = Instantiate(wallAsset);
                wall.transform.localPosition = new Vector3(wallX, 0, wallY);
                wall.name = "Wall cap";
                wall.transform.SetParent(transform);
                walls.Add(wall);
            }
        }

        private void ConstructCorridor(GameObject corridorAsset, GameObject floorAsset, Rect path, DungeonAxis axis)
        {
            // Don't place anything if corridor has no height or width. This means the rooms share a boundary
            if ((Mathf.Approximately(path.width, 0) || Mathf.Approximately(path.height, 0)) && Paths.Count == 1)
            {
                transform.position = new(path.x, 0, path.y);
                return;
            }

            GameObject floor = Instantiate(floorAsset);
            floor.transform.SetParent(transform);
            floor.transform.localScale = DungeonGeneratorUtils.Vec2ToVec3(path.size, 0.5f);
            floor.transform.localPosition = DungeonGeneratorUtils.Vec2ToVec3(path.position, 0);

            int count = Mathf.RoundToInt(axis == DungeonAxis.VERTICAL ? path.height : path.width);

            for (int i = 0; i < count; i++)
            {
                float wallX = axis == DungeonAxis.VERTICAL ? path.position.x : path.position.x + i;
                float wallY = axis == DungeonAxis.VERTICAL ? path.position.y + i : path.position.y;

                GameObject wall = Instantiate(corridorAsset);
                wall.transform.localPosition = new Vector3(wallX, 0, wallY);
                wall.name = "Corridor";
                wall.transform.SetParent(transform);
                walls.Add(wall);

                wallX = axis == DungeonAxis.VERTICAL ? path.position.x + path.size.x : path.position.x + i;
                wallY = axis == DungeonAxis.VERTICAL ? path.position.y + i : path.position.y + path.size.y;

                GameObject wall2 = Instantiate(corridorAsset);
                wall2.transform.localPosition = new Vector3(wallX, 0, wallY);
                wall2.name = "Corridor";
                wall2.transform.SetParent(transform);

                walls.Add(wall2);
            }
        }

        internal static DungeonCorridor Create(List<Tuple<Rect, DungeonAxis>> paths)
        {
            // Create gameobject code referenced from  - https://discussions.unity.com/t/how-do-you-create-an-empty-gameobject-in-code-and-add-it-to-the-scene/86380/4
            GameObject gameObj = new("Corridor " + corridorId++);
            DungeonCorridor corridor = gameObj.AddComponent<DungeonCorridor>();
            corridor.Paths = paths;
            return corridor;
        }

        internal void Modify(Rect bounds)
        {
            Debug.Log("Bounds = " + bounds);
            foreach (var wall in walls)
            {
                Vector3 wallPos = wall.transform.position;
                if ((wallPos.x > bounds.x && wallPos.x < bounds.xMax) && (wallPos.z > bounds.y && wallPos.z < bounds.yMax))
                {
                    wall.SetActive(false);
                    Debug.Log("wall pos = " + wall.transform.position);
                }
            }
        }
    }
}