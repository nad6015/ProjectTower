using Assets.PlayerCharacter;
using Assets.Scripts.Environment;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static Assets.Utilities.GameObjectUtilities;

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
            _prompt.GetComponent<TextMeshPro>().text = "";
        }

        public DoorKey LockDoor(DoorKey doorKey)
        {
            _doorKey = doorKey;
            _locked = true;
            _animator.SetBool("Locked", true);
            _prompt.GetComponent<TextMeshPro>().text = "Unlock(E)";

            return _doorKey;
        }

        protected override void HandleInteract(InputAction.CallbackContext context)
        {
            PlayerInventory playerInventory = _other.GetComponent<PlayerInventory>();
            if (_locked && playerInventory.Contains(_doorKey))
            {
                _locked = false;
                _animator.SetBool("Locked", _locked);
                _prompt.GetComponent<TextMeshPro>().text = "";
            }
        }
    }
}