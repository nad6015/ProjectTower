using Assets.DungeonGenerator.Components;
using System;
using UnityEngine;

namespace Assets.DungeonGenerator
{
	/**
	 * Interface for all dungeon algorithms. Used to ensure all algorithms have a consistent contract.
	 */
	internal interface IDungeonAlgorithm
	{
		void GenerateRepresentation(Dungeon dungeon);
        void ConstructDungeon(DungeonComponents room);
        void PlaceContent(DungeonComponents components);
    }
}