using Assets.Items;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.PlayerCharacter
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField]
        int maxItemStack;
        private Dictionary<IItem, int> inventory;

        void Awake()
        {
            inventory = new Dictionary<IItem, int>();
        }
    }
}