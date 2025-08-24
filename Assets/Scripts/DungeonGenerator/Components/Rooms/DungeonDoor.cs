using Assets.Interactables;
using Assets.PlayerCharacter;
using TMPro;
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
            prompt.GetComponent<TextMeshPro>().text = "";
        }

        public DoorKey LockDoor(DoorKey doorKey)
        {
            _doorKey = doorKey;
            _locked = true;
            _animator.SetBool("Locked", true);
            prompt.GetComponent<TextMeshPro>().text = "Unlock(E)";

            return _doorKey;
        }

        protected override void HandleInteract(InputAction.CallbackContext context)
        {
            PlayerInventory playerInventory = controller.GetComponent<PlayerInventory>();
            if (_locked && playerInventory.Contains(_doorKey))
            {
                _locked = false;
                _animator.SetBool("Locked", _locked);
                prompt.GetComponent<TextMeshPro>().text = "";
            }
        }
    }
}