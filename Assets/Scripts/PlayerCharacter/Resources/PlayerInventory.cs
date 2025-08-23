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
        private int _maxItemStack;

        private Dictionary<UsableItem, int> _inventory;

        void Awake()
        {
            _inventory = new Dictionary<UsableItem, int>();
        }

        public void Add(UsableItem gameObject)
        {
            if (_inventory.ContainsKey(gameObject))
            {
                _inventory[gameObject]++;
                _inventory[gameObject] = Mathf.Min(_inventory[gameObject], _maxItemStack);
            }
            else
            {
                _inventory.Add(gameObject, 1);
                DisplayItem(gameObject);
            }
        }

        private void DisplayItem(UsableItem gameObject)
        {
            foreach (var itemSlot in _visibleInventory)
            { 
                if(itemSlot.transform.childCount == 0)
                {
                    gameObject.transform.SetParent(itemSlot.transform, false);
                }
            }
        }

        public bool Contains(UsableItem item)
        {
            return _inventory.ContainsKey(item);
        }
    }
}