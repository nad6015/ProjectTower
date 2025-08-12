using Assets.PlayerCharacter;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Environment
{
    /// <summary>
    /// Base class for any interactable item with the game environment.
    /// </summary>
    public abstract class Interactable : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerController>().OnInteract += HandleInteract;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerController>().OnInteract -= HandleInteract;
            }
        }

        protected abstract void HandleInteract(InputAction.CallbackContext context);
    }
}