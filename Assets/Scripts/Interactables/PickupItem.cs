using Assets.PlayerCharacter;
using Assets.PlayerCharacter.Resources;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Interactables
{
    public class PickupItem : Interactable
    {
        [field: SerializeField]
        public UsableItem Pickup { get; set; }

        private float _rotationSpeed = 1f;
        private void Update()
        {
            transform.Rotate(Vector3.up, _rotationSpeed);
        }

        protected override void HandleInteract(InputAction.CallbackContext context)
        {
            Pickup.Use(controller);
        }
    }
}