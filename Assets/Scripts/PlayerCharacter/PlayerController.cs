using Assets.Scripts.Combat.Resources;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.PlayerCharacter
{
    [RequireComponent(typeof(CharacterController), typeof(PlayerAction), typeof(PlayerMovement))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private float _speed = 5;
        
        [SerializeField]
        private PlayerCamera _camera;

        public event Action<InputAction.CallbackContext> OnAttackPerformed;

        private PlayableFighter _fighter;

        private InputSystemActions actions;
        private PlayerMovement playerMovement;
        private PlayerAction playerAction;

        void Awake()
        {
            playerMovement = GetComponent<PlayerMovement>();
            playerMovement.speed = _speed;

            playerAction = GetComponent<PlayerAction>();
            actions = new InputSystemActions();
            _fighter = GetComponent<PlayableFighter>();

            Instantiate(_camera);
        }

        private void OnEnable()
        {
            actions.Player.Move.Enable();
            actions.Player.Move.performed += playerMovement.OnMovePerformed;
            actions.Player.Move.canceled += playerMovement.OnMoveCancelled;

            actions.Player.Attack.Enable();
            actions.Player.Attack.performed += AttackPerformed;
            //actions.Player.Attack.canceled += playerAction.OnAttackCancelled;
        }

        private void OnDisable()
        {
            actions.Player.Move.Disable();
            actions.Player.Move.performed -= playerMovement.OnMovePerformed;
            actions.Player.Move.canceled -= playerMovement.OnMoveCancelled;

            actions.Player.Attack.Disable();
            actions.Player.Attack.performed -= AttackPerformed;
            //actions.Player.Attack.canceled -= playerAction.OnAttackCancelled;
        }

        private void AttackPerformed(InputAction.CallbackContext context)
        {
            OnAttackPerformed?.Invoke(context);
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