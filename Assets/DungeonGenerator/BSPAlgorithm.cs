using UnityEngine;
using Assets.DungeonGenerator.Components;
using System.Linq.Expressions;
using System;
using Unity.Mathematics;
using System.Collections.Generic;

namespace Assets.DungeonGenerator
{
    using Random = UnityEngine.Random;
    internal class BSPAlgorithm : IDungeonAlgorithm
    {
        public BSPNode bspTreeRoot;
        private List<BSPNode> bspTree;
        private Dungeon dungeon;

        public void GenerateRepresentation(Dungeon dungeon)
        {
            bspTree = new List<BSPNode>();

            this.dungeon = dungeon;
            bspTreeRoot = new BSPNode(this.dungeon);

            PartitionSpace(bspTreeRoot, RandomAxis());

            /* Generate room representation:
             * 1. Choose a random size within the min and max bounds(note: this could be a parameter)
             * 2. Choose a random axis(verticle or horizontal) and divide along it(coords can be random)
             * 3. Choose one of the two paritions then repeat steps 2 and 3 until min room bound is met.
             * 4. Repeat for the other partition.
             */
        }

        private void PartitionSpace(BSPNode node, DungeonAxis axis)
        {
            bspTree.Add(node);
            if (node.IsRoomMinSize(dungeon.MinRoomSize))
            {
                return;
            }
            else
            {
                CreateNewNodes(node, axis);

                DungeonAxis childNodeAxis = RandomAxis();

                PartitionSpace(node.Left, childNodeAxis);

                if (node.Right != null)
                {
                    PartitionSpace(node.Right, childNodeAxis);
                }
            }
        }

        private void CreateNewNodes(BSPNode node, DungeonAxis axis)
        {
            Rect nodeSize = node.Bounds;
            float x = nodeSize.x;
            float y = nodeSize.y;

            float width = nodeSize.width;
            float height = nodeSize.height;

            float rightWidth = nodeSize.width;
            float rightHeight = nodeSize.height;

            bool shouldCreateOneNode = false;

            if (axis == DungeonAxis.HORIZONTAL)
            {
                height = Mathf.Round(Random.Range(dungeon.MinRoomSize.y, node.Bounds.height));
                rightHeight -= height;

                if (rightHeight < dungeon.MinRoomSize.y)
                {
                    height -= dungeon.MinRoomSize.y;
                    rightHeight = dungeon.MinRoomSize.y;
                }

                if (height + rightHeight < nodeSize.height)
                {
                    height = nodeSize.height - rightHeight;
                }

                y += height;
                shouldCreateOneNode = height < dungeon.MinRoomSize.y && rightHeight >= dungeon.MinRoomSize.y;
            }
            else
            {
                width = Mathf.Round(Random.Range(dungeon.MinRoomSize.x, node.Bounds.width));
                rightWidth -= width;
                if (rightWidth < dungeon.MinRoomSize.x)
                {
                    width -= dungeon.MinRoomSize.x;
                    rightWidth = dungeon.MinRoomSize.x;
                }

                if (width + rightWidth < nodeSize.width)
                {
                    width = nodeSize.width - rightWidth;
                }

                x += width;
                shouldCreateOneNode = width < dungeon.MinRoomSize.x && rightWidth >= dungeon.MinRoomSize.x;
            }

            if (shouldCreateOneNode) {
                width = Mathf.Round(Random.Range(dungeon.MinRoomSize.x,nodeSize.width-1));
                height = Mathf.Round(Random.Range(dungeon.MinRoomSize.y, nodeSize.height - 1));
            }

            Rect leftSpace = new(nodeSize.x, nodeSize.y, width, height);
            new BSPNode(node, leftSpace);

            if (!shouldCreateOneNode)
            {
                Rect rightSpace = new(x, y, rightWidth, rightHeight);
                new BSPNode(node, rightSpace);
            }
        }

        private DungeonAxis RandomAxis()
        {
            if (Random.value > 0.5)
            {
                return DungeonAxis.VERTICAL;
            }
            return DungeonAxis.HORIZONTAL;
        }

        public void ForEachRoom(Action<DungeonRoom> forEachFunc)
        {
            bspTree.ForEach(node =>
            {
                if (node.IsRoomMinSize(dungeon.MinRoomSize))
                {
                    forEachFunc.Invoke(node.GenerateRoom());
                }
            });
        }

        private enum DungeonAxis
        {
            HORIZONTAL,
            VERTICAL
        }
    }
}