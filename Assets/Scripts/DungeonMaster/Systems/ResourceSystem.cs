using Assets.DungeonGenerator.Components;
using Assets.DungeonGenerator.DataStructures;
using Assets.DungeonMaster;
using Assets.Interactables;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Combat
{
    public class ResourceSystem : MonoBehaviour, IDungeonResourceManager
    {
        private float _itemDropRate;

        [SerializeField]
        private List<PickupItem> _containerItemsList;

        private Shufflebag<PickupItem> _containerItems;

        public void Awake()
        {
            _containerItems = new(_containerItemsList);
        }

        public Interactable TakeContainerItem()
        {
            Debug.Log(_itemDropRate);
            if (_itemDropRate > Random.value)
            {
                return _containerItems.TakeItem();
            }
            return null;
        }

        public void UpdateItemRates(Dictionary<GameplayParameter, ValueRepresentation> newRates)
        {
            _itemDropRate = newRates[GameplayParameter.restorationItem].Value<int>();
        }
    }
}