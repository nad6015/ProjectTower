using System.Collections.Generic;
using UnityEngine;

namespace Assets.DungeonGenerator.DataStructures
{
    /// <summary>
    /// A C# representation of a shufflebag, a method of controlling randomness to ensure fairness in games.
    /// A set of outcomes are put into the bag and then radnomly pulled out. For example, if a dice roll should 
    /// have a 1/6 chance of landing on any face, then the bag would have 1,2,3,4,5,6 which would be pulled out randomly.
    /// Once the bag is empty, the selected outcomes would be put back into the bag.
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
               RefreshBag();
            }

            int randomIndex = Mathf.RoundToInt(Random.value * (_shuffleBag.Count - 1));

            T item = _shuffleBag[randomIndex];
            _shuffleBag.RemoveAt(randomIndex);

            return item;
        }

        /// <summary>
        /// Refills the shufflebag without taking any items from it.
        /// </summary>
        public void RefreshBag()
        {
            _shuffleBag = new List<T>(_originalList);
        }
    }
}