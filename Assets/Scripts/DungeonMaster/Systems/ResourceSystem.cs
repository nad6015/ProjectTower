using Assets.DungeonGenerator.Components;
using Assets.DungeonGenerator.Components.Tiles;
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
        [SerializeField]
        private TreasureChest chest;

        [SerializeField]
        private List<PickupItem> _containerItemsList;

        [SerializeField]
        private List<UsableItem> _treasureItemsList;

        private float ItemDropRate { get; set; }
        private float _treasureDropRate;
        private bool _hasPlacedTreasure = false;

        private Shufflebag<PickupItem> _containerItems;
        private Shufflebag<UsableItem> _treasureItems;

        public void Awake()
        {
            _containerItems = new(_containerItemsList);
            _treasureItems = new(_treasureItemsList);
        }

        /// <summary>
        /// Randomly take one interactable item from the list of available items.
        /// If the item drop rate is lower than a randomly selected number, then don't
        /// spawn anything.
        /// </summary>
        /// <returns>An Interactable</returns>
        public Interactable TakeContainerItem()
        {
            if (ItemDropRate > Random.value)
            {
                return _containerItems.TakeItem();
            }
            return null;
        }

        /// <summary>
        /// Randomly take one treasure chest item from the list of available treasures.
        /// </summary>
        /// <returns>A UsableItem</returns>
        public UsableItem TakeTreasureChestItem()
        {
            return _treasureItems.TakeItem();
        }

        /// <summary>
        /// Updates the items rates based on the gameplay parameters dictionary.
        /// </summary>
        /// <param name="newRates">the new rates in a dictionary</param>
        public void UpdateItemRates(Dictionary<GameplayParameter, ValueRepresentation> newRates)
        {
            ItemDropRate = newRates[GameplayParameter.ItemDropRate].Value<int>() / 100f;

            float newTreasureDropRate = newRates[GameplayParameter.TreasureDropRate].Value<int>() / 100f;

            // if treasure drop rate has changed then check chance to spawn treasure
            if (newTreasureDropRate != _treasureDropRate)
            {
                _treasureDropRate = newTreasureDropRate;
                if (!_hasPlacedTreasure && _treasureDropRate > Random.value)
                {
                    DungeonEndRoom room = GameObject.FindFirstObjectByType<DungeonEndRoom>();
                    Vector3 pos = new Vector3(room.Bounds.center.x, 0, room.Bounds.center.z - DungeonTilemap.TileUnit * 2);
                    GameObject.Instantiate(chest, pos + chest.transform.position, Quaternion.identity, room.transform);
                    _hasPlacedTreasure = true;
                    _treasureDropRate = 0;
                }
            }
        }

        /// <summary>
        /// Resets the state of the resource system. Typically called when a dungeon is generated.
        /// </summary>
        public void Reset()
        {
            _hasPlacedTreasure = false;
        }
    }
}