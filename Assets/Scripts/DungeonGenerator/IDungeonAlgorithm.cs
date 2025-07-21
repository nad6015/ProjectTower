using Assets.DungeonGenerator.Components;
using System;
using UnityEngine;

namespace Assets.DungeonGenerator
{
    /// <summary>
    /// Interface for all dungeon algorithms. Used to ensure all algorithms have a consistent contract.
    /// </summary>
    internal interface IDungeonAlgorithm
	{
        void GenerateDungeon();
    }
}