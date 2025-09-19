using System.Collections.Generic;
using UnityEngine;

namespace Assets.DungeonMaster
{
    internal class AdaptionManager
    {
        public Dictionary<GameParameter, int> FloorStatistics { get; private set; }

        internal AdaptionManager()
        {
            FloorStatistics = new();
        }

        /// <summary>
        /// Updates the statistics held by this AdaptionManager.
        /// </summary>
        /// <param name="param">The param to update</param>
        /// <param name="value">The value the param will increase by</param>
        internal void UpdateGameStatistics(GameParameter param, int value)
        {
            if (!FloorStatistics.TryAdd(param, value))
            {
                FloorStatistics[param] += value;
            }
        }

        /// <summary>
        /// Resets a floor statistics to zero.
        /// </summary>
        /// <param name="gameParameter"></param>
        internal void Reset(GameParameter gameParameter)
        {
            FloorStatistics[gameParameter] = 0;
        }
    }
}