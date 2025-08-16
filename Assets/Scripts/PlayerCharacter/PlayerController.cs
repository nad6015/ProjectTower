using Assets.Scripts.Combat.Resources;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.PlayerCharacter
{
    [RequireComponent(typeof(CharacterController), typeof(PlayerMovement))]
    public class PlayerController : MonoBehaviour
    {
        public event Action<InputAction.CallbackContext> OnInteract;
        public event Action<InputAction.CallbackContext> OnAttackPerformed;
        public event Action<InputAction.CallbackContext> MovePerformed;
        public event Action<InputAction.CallbackContext> MoveCancelled;

        [SerializeField]
        private GameObject _camera;

        private PlayableFighter _fighter;

        private InputSystemActions _actions;
        private PlayerMovement _playerMovement;

        // Using both Awake and Start here to first initialise the controller's values (Awake) for the OnEnable and OnDisable
        // and then to set the referenced scripts' values (Start).
        void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            _fighter = GetComponent<PlayableFighter>();

            _actions = new InputSystemActions();
            Instantiate(_camera);
        }

        private void Start()
        {
            _playerMovement.speed = _fighter.GetMaxStat(Combat.FighterStats.Speed);
        }

        private void OnEnable()
        {
            _actions.Player.Move.Enable();
            _actions.Player.Move.performed += MovePerformed;
            _actions.Player.Move.canceled += MoveCancelled;

            _actions.Player.Attack.Enable();
            _actions.Player.Attack.performed += AttackPerformed;

            _actions.Player.Interact.Enable();
            _actions.Player.Interact.performed += InteractPerformed;
        }

        private void OnDisable()
        {
            _actions.Player.Move.Disable();
            _actions.Player.Move.performed -= MovePerformed;
            _actions.Player.Move.canceled -= MoveCancelled;

            _actions.Player.Attack.Disable();
            _actions.Player.Attack.performed -= AttackPerformed;

            _actions.Player.Interact.Disable();
            _actions.Player.Interact.performed -= InteractPerformed;
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