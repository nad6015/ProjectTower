using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField]
    int maxItemStack;
    private Dictionary<IItem, int> inventory;

    void Awake()
    {
        inventory = new Dictionary<IItem, int>();
    }

    interface IItem
    {
        string Name { get; }
        void Use();
    }
}
