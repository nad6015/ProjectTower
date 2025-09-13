using UnityEngine;
using Unity;
using UnityEngine.InputSystem;
using System;

namespace Assets.PlayerCharacter
{
    public class PlayerMovement : MonoBehaviour
    {
        internal float speed { get; set; }
        private CharacterController _characterController;
        private Vector3 newPos;
        private Quaternion _currentRotation;
        private Animator _animator;
        private PlayableFighter _fighter;

        void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _fighter = GetComponent<PlayableFighter>();

            PlayerController playerController = GetComponent<PlayerController>();
            playerController.MovePerformed += OnMovePerformed;
            playerController.MoveCancelled += OnMoveCancelled;

            _animator = GetComponentInChildren<Animator>();
            _animator.SetFloat("MotionSpeed", 1);

            Cursor.lockState = CursorLockMode.Confined;
            _currentRotation = transform.rotation;
        }

        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<Vector2>();
            newPos.x = value.x;
            newPos.z = value.y;
            _animator.SetFloat("Speed", speed);

        }

        private void OnMoveCancelled(InputAction.CallbackContext context)
        {
            newPos = Vector3.zero;
            _animator.SetFloat("Speed", 0);
        }

        void Update()
        {
            _currentRotation = Quaternion.LookRotation(newPos == Vector3.zero ? transform.forward : newPos, Vector3.up);

            transform.rotation = _currentRotation;

            _characterController.SimpleMove(speed * newPos);
        }
    }
}
