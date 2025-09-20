using Assets.DungeonGenerator.DataStructures;
using Assets.Interactables;
using Assets.PlayerCharacter;
using Assets.PlayerCharacter.Resources;
using UnityEngine;
using UnityEngine.InputSystem;
using static Assets.Utilities.GameObjectUtilities;

namespace Assets.DungeonGenerator.Components
{
    public class TreasureChest : Interactable
    {
        private IDungeonResourceManager _manager;
        private Animator _animator;
        private UsableItem _item;

        private void Start()
        {
            _manager = FindComponentByTag<IDungeonResourceManager>("ResourceSystem");
            _animator = GetComponentInChildren<Animator>();
            _item = _manager.TakeTreasureChestItem();
            string stat = "";

            switch (((StatIncreaser)_item).Stats)
            {
                case Combat.FighterStats.Health:
                {
                    stat = "health";
                    break;
                }
                case Combat.FighterStats.Stamina:
                {
                    stat = "stamina";
                    break;
                }
                case Combat.FighterStats.Speed:
                {
                    stat = "speed";
                    break;
                }
                case Combat.FighterStats.Attack:
                {
                    stat = "attack";
                    break;
                }
            }
            prompt = "Open to increase " + stat + " by 1";
        }

        protected override void HandleInteract(InputAction.CallbackContext context)
        {
            Open();
        }

        public void Open()
        {
            _item.Use(controller);
            _animator.SetBool("Open", true);
            DisableInteraction();
        }
    }
}