using Assets.Scripts.Combat.Resources;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.PlayerCharacter
{
    [RequireComponent(typeof(CharacterController), typeof(PlayerAction), typeof(PlayerMovement))]
    public class PlayerController : MonoBehaviour
    {
        public event Action<InputAction.CallbackContext> OnInteract;
        public event Action<InputAction.CallbackContext> OnAttackPerformed;
        [SerializeField]
        private float _speed = 5;
        
        [SerializeField]

        private PlayerCamera _camera;

        private PlayableFighter _fighter;

        private InputSystemActions actions;
        private PlayerMovement playerMovement;
        private PlayerCamera _playerCamera;
        

        void Awake()
        {
            playerMovement = GetComponent<PlayerMovement>();
            playerMovement.speed = _speed;

            actions = new InputSystemActions();
            _fighter = GetComponent<PlayableFighter>();

            _playerCamera = Instantiate(_camera);
        }

        private void OnEnable()
        {
            actions.Player.Move.Enable();
            actions.Player.Move.performed += playerMovement.OnMovePerformed;
            actions.Player.Move.canceled += playerMovement.OnMoveCancelled;

            actions.Player.Attack.Enable();
            actions.Player.Attack.performed += AttackPerformed;

            actions.Player.Interact.Enable();
            actions.Player.Interact.performed += InteractPerformed;
        }

        private void OnDisable()
        {
            actions.Player.Move.Disable();
            actions.Player.Move.performed -= playerMovement.OnMovePerformed;
            actions.Player.Move.canceled -= playerMovement.OnMoveCancelled;

            actions.Player.Attack.Disable();
            actions.Player.Attack.performed -= AttackPerformed;

            actions.Player.Interact.Disable();
            actions.Player.Interact.performed -= InteractPerformed;
        }

        private void AttackPerformed(InputAction.CallbackContext context)
        {
            OnAttackPerformed?.Invoke(context);
        }

        private void InteractPerformed(InputAction.CallbackContext context)
        {
            OnInteract?.Invoke(context);
        }

        private void OnTriggerEnter(Collider other)
        {
            switch (other.tag)
            {
                case "Item":
                {
                    other.GetComponent<Item>().Use(_fighter);
                    break;
                }
            }
        }
    }
}