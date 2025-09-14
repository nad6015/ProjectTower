using Assets.DungeonGenerator.Components;
using Assets.DungeonGenerator.DataStructures;
using Assets.DungeonMaster;
using Assets.Interactables;
using Assets.PlayerCharacter.Resources;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Combat
{
    public class ResourceSystem : MonoBehaviour, IDungeonResourceManager
    {
        private float _itemDropRate;
        private float _treasureDropRate;

        [SerializeField]
        private List<PickupItem> _containerItemsList;

        [SerializeField]
        private List<UsableItem> _treasureItemsList;

        private Shufflebag<PickupItem> _containerItems;
        private Shufflebag<UsableItem> _treasureItems;

        public void Awake()
        {
            _containerItems = new(_containerItemsList);
            _treasureItems = new(_treasureItemsList);
        }

        public Interactable TakeContainerItem()
        {
            if (_itemDropRate > Random.value)
            {
                return _containerItems.TakeItem();
            }
            return null;
        }

        public UsableItem TakeTreasureChestItem()
        {
            if (_treasureDropRate > Random.value)
            {
                return _treasureItems.TakeItem();
            }
            return null;
        }

        public void UpdateItemRates(Dictionary<GameplayParameter, ValueRepresentation> newRates)
        {
            _itemDropRate = newRates[GameplayParameter.restorationItem].Value<int>();
        }
    }
}