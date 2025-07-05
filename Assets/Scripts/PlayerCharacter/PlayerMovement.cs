using UnityEngine;
using Unity;
using UnityEngine.InputSystem;

namespace Assets.PlayerCharacter
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField]
        private AudioClip footstepClip;
        internal float speed { get; set; }
        private CharacterController characterController;
        private Vector3 newPos;
        private Quaternion currentRotation;
        private Animator animator;
        private AudioSource audioSource;


        void Awake()
        {
            characterController = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();
            animator.SetFloat("MotionSpeed", 1);

            Cursor.lockState = CursorLockMode.Confined;
            currentRotation = transform.rotation;
            audioSource = GetComponent<AudioSource>();
        }

        internal void OnMovePerformed(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<Vector2>();
            newPos.x = value.x;
            newPos.z = value.y;
            animator.SetFloat("Speed", speed);

        }

        internal void OnMoveCancelled(InputAction.CallbackContext context)
        {
            newPos = Vector3.zero;
            animator.SetFloat("Speed", 0);
        }

        void Update()
        {
            currentRotation = Quaternion.LookRotation(newPos == Vector3.zero ? transform.forward : newPos, Vector3.up);

            transform.rotation = currentRotation;

            characterController.SimpleMove(speed * newPos);
        }

        public void OnFootstep()
        {
            audioSource.clip = footstepClip;
            audioSource.Play();
        }
    }
}
