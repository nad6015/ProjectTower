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
        [SerializeField]
        private PickupItem _chestItem;

        private IDungeonResourceManager _manager;
        private Animator _animator;

        private void Start()
        {
            _manager = FindComponentByTag<IDungeonResourceManager>("ResourceSystem");
            _animator = GetComponentInChildren<Animator>();
        }

        protected override void HandleInteract(InputAction.CallbackContext context)
        {
            Open();
        }

        public void Open()
        {
            _chestItem.gameObject.SetActive(true);
            _animator.SetBool("Open", true);
            DisableInteraction();
        }
    }

    public class StatIncreaser : UsableItem
    {
        public override void Use(PlayerController player)
        {
            throw new System.NotImplementedException();
        }
    }
}