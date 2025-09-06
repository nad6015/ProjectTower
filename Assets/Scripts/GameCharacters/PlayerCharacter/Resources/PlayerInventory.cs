using Assets.PlayerCharacter.Resources;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.PlayerCharacter
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> _visibleInventory;

        [SerializeField]
        private int _maxInventorySize = 3;

        private List<UsableItem> _inventory;

        void Awake()
        {
            _inventory = new();
        }

        public void Add(UsableItem gameObject)
        {
            if (!_inventory.Contains(gameObject) && _inventory.Count < _maxInventorySize)
            {
                _inventory.Add(gameObject);
                DisplayItem(gameObject);
            }
        }

        /// <summary>
        /// Displays the usable item in the first available inventory slot
        /// </summary>
        /// <param name="item"></param>
        private void DisplayItem(UsableItem item)
        {
            foreach (var itemSlot in _visibleInventory)
            {
                if (itemSlot.transform.childCount == 0)
                {
                    item.transform.SetParent(itemSlot.transform, false);
                }
            }
        }

        /// <summary>
        /// Removes the given item from the inventory display.
        /// </summary>
        /// <param name="item"></param>
        private void RemoveItemFromDisplay(UsableItem item)
        {
            foreach (var itemSlot in _visibleInventory)
            {
                if (itemSlot.transform.GetChild(0).gameObject == item.gameObject)
                {
                    item.transform.SetParent(null);
                    item.gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Checks if the given item exists within the inventory.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(UsableItem item)
        {
            return _inventory.Contains(item);
        }

        /// <summary>
        /// Removes the specified item from the inventory if it exists.
        /// Removing an item with multiple instances within the inventory, only decreases the number of instances.
        /// If the item is the last in the inventory, it is removed from display as well as from the inventory map.
        /// </summary>
        /// <param name="item"></param>
        public void Remove(UsableItem item)
        {
            if (_inventory.Contains(item))
            {
                _inventory.Remove(item);
                RemoveItemFromDisplay(item);
            }
        }
    }
}