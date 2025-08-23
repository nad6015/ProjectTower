using System.Collections.Generic;
using UnityEngine;

namespace Assets.Combat
{
    public class ResourceSystem : MonoBehaviour
    {
        [SerializeField]
        private float _itemDropRate;
        [SerializeField]
        //private List<RestorationItem> _restorationItems;

        //private ItemDrop<RestorationItem> _restorationDrops;
        public void Awake()
        {
            //_restorationDrops = new(_itemDropRate, _restorationItems);
        }
    }

    public class ItemDrop<T>
    {
        public float Rate { get; }
        // The item drops and their respective drop chance 
        private List<T> _spawnItems;

        public ItemDrop(float rate, List<T> spawnItems)
        {
            Rate = rate;
            _spawnItems = spawnItems;
        }

        /// <summary>
        /// Increases the chance of an item being spawned by adding it to the list again.
        /// </summary>
        /// <param name="item"></param>
        public void SkewDropChance(T item)
        {
            int index = _spawnItems.IndexOf(item); // Find the item to add to the list

            if (index == -1)
            {
                return; // If item not in drops, then do nothing.
            }

            _spawnItems.Add(item);
        }

        public T Drop()
        {
            int randomVal = Random.Range(0, _spawnItems.Count -1);

            return _spawnItems[randomVal];
        }
    }
}