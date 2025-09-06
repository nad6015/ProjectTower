using Assets.Interactables;
using Assets.PlayerCharacter;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.DungeonGenerator.Components
{
    public class DungeonDoor : Interactable
    {
        [SerializeField]
        private bool _locked;

        private DoorKey _doorKey;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        public DoorKey LockDoor(DoorKey doorKey)
        {
            _doorKey = doorKey;
            _locked = true;
            _animator.SetBool("Locked", true);

            return _doorKey;
        }

        protected override void HandleInteract(InputAction.CallbackContext context)
        {
            PlayerInventory playerInventory = controller.GetComponent<PlayerInventory>();
            if (_locked && playerInventory.Contains(_doorKey))
            {
                _locked = false;
                _animator.SetBool("Locked", _locked);
            }
        }
    }
}