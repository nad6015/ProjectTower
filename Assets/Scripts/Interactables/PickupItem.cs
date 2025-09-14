using Assets.PlayerCharacter.Resources;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Interactables
{
    public class PickupItem : Interactable
    {
        [field: SerializeField]
        public UsableItem Pickup { get; private set; }

        private readonly float _rotationSpeed = 1f;
        private void Awake()
        {
            prompt = "Pickup";
        }
        private void Update()
        {
            Pickup.transform.Rotate(Vector3.up, _rotationSpeed);
        }

        protected override void HandleInteract(InputAction.CallbackContext context)
        {
            Pickup.Use(controller);
            DisableInteraction();
            gameObject.SetActive(false);
        }
    }
}