using System.Collections.Generic;
using UnityEngine;

namespace Assets.DungeonGenerator.Components.Tiles
{
    /// <summary>
    /// A C# representation of a shufflebag (reference: https://medium.com/@lemapp09/beginning-game-development-shuffle-bag-d059a3f38bfc)
    /// for creating controlled randomness.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Shufflebag<T>
    {
        private List<T> _originalList;
        private List<T> _shuffleBag;
        public Shufflebag(List<T> originalList)
        {
            _originalList = new List<T>(originalList);
            _shuffleBag = new List<T>(originalList);
        }

        /// <summary>
        /// Randomly takes and returns an item from the shuffle bag. If the bag is empty, then repopulates
        /// it with the original values before taking an item.
        /// </summary>
        /// <returns>A randomly chosen item from the shufflebag.</returns>
        public T TakeItem()
        {
            if (_shuffleBag.Count == 0 && _originalList.Count == 0)
            {
                return default;
            }
            else if (_shuffleBag.Count == 0)
            {
                _shuffleBag = new List<T>(_originalList);
            }

            int randomIndex = Mathf.RoundToInt(Random.value * (_shuffleBag.Count - 1));

            T item = _shuffleBag[randomIndex];
            _shuffleBag.RemoveAt(randomIndex);

            return item;
        }
    }
}