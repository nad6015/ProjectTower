//using Assets.PlayerCharacter;
//using UnityEngine;
//using UnityEngine.InputSystem;

//namespace Assets.Scripts.Environment
//{
//    /// <summary>
//    /// Base class for any interactable item within the game environment.
//    /// </summary>
//    public abstract class Interactable : MonoBehaviour
//    {
//        [SerializeField]
//        protected GameObject _prompt;
//        protected GameObject _other;

//        private void Start()
//        {
//            _prompt.SetActive(false);
//        }

//        private void OnTriggerEnter(Collider other)
//        {
//            if (other.CompareTag("Player"))
//            {
//                other.GetComponent<PlayerController>().OnInteract += HandleInteract;
//                _prompt.SetActive(true);
//                _other = other.gameObject;
//            }
//        }

//        private void OnTriggerExit(Collider other)
//        {
//            if (other.CompareTag("Player"))
//            {
//                other.GetComponent<PlayerController>().OnInteract -= HandleInteract;
//                _prompt.SetActive(false);
//                other = null;
//            }
//        }

//        protected abstract void HandleInteract(InputAction.CallbackContext context);
//    }
//}