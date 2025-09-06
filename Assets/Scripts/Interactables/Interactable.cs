using Assets.PlayerCharacter;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Interactables
{
    /// <summary>
    /// Base class for any interactable item within the game environment.
    /// </summary>
    public abstract class Interactable : MonoBehaviour
    {
        protected PlayerController controller;

        private void OnDisable()
        {
            if (controller != null)
            {
                controller.OnInteract -= HandleInteract;
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                controller = other.GetComponent<PlayerController>();
                controller.OnInteract += HandleInteract;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                controller.OnInteract -= HandleInteract;
            }
        }

        protected abstract void HandleInteract(InputAction.CallbackContext context);
    }
}