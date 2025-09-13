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
        protected string prompt = "Interact";

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
                controller.ShowHUD(prompt);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                DisableInteraction();
            }
        }

        protected void DisableInteraction()
        {
            controller.OnInteract -= HandleInteract;
            controller.HideHUD();
        }

        protected abstract void HandleInteract(InputAction.CallbackContext context);
    }
}