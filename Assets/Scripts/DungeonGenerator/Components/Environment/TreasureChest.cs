using Assets.DungeonGenerator.DataStructures;
using Assets.Interactables;
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

        private void Start()
        {
            _manager = FindComponentByTag<IDungeonResourceManager>("ResourceSystem");
            _animator = GetComponentInChildren<Animator>();
            prompt = "Open";
        }

        protected override void HandleInteract(InputAction.CallbackContext context)
        {
            Open();
        }

        public void Open()
        {
            UsableItem item = _manager.TakeTreasureChestItem();
            item.Use(controller);
            _animator.SetBool("Open", true);
            DisableInteraction();
        }
    }
}