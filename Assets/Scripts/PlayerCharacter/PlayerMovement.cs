using UnityEngine;
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
            animator = GetComponent<Animator>();
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
            // Rotation Code referenced from - https://discussions.unity.com/t/make-a-player-model-rotate-towards-mouse-location/125354/3

            Vector2 transformOnScreen = Camera.main.WorldToViewportPoint(transform.position);
            Vector2 mouseOnScreen = Camera.main.ScreenToViewportPoint(Mouse.current.position.ReadValue());

            float x = Unity.Mathematics.math.remap(0, 1, -1, 1, mouseOnScreen.x);
            float y = Unity.Mathematics.math.remap(0, 1, -1, 1, mouseOnScreen.y);
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
