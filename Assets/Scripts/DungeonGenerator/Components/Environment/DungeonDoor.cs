using Assets.Interactables;
using Assets.PlayerCharacter;
using System;
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
            prompt = "Unlock";
        }

        internal DoorKey LockDoor(DoorKey doorKey)
        {
            _doorKey = doorKey;
            _locked = true;
            _animator.SetBool("Locked", true);

            return _doorKey;
        }

        internal void LockDoor()
        {
            _locked = true;
            _animator.SetBool("Locked", true);
        }

        protected override void HandleInteract(InputAction.CallbackContext context)
        {
            PlayerInventory playerInventory = controller.GetComponent<PlayerInventory>();
            if (_locked && playerInventory.Contains(_doorKey))
            {
                Unlock();
            }
            playerInventory.Remove(_doorKey);
        }

        internal void Unlock()
        {
            _locked = false;
            _animator.SetBool("Locked", _locked);
            DisableInteraction();
            prompt = "";
        }
    }
}